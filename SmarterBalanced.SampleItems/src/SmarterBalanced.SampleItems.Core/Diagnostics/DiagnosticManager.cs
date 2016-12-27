using Amazon.EC2;
using Amazon.EC2.Model;
using Amazon.Runtime;
using Microsoft.Extensions.Logging;
using SmarterBalanced.SampleItems.Core.Diagnostics.Models;
using SmarterBalanced.SampleItems.Dal.Providers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Amazon.ECS;
using ecsModel = Amazon.ECS.Model;

namespace SmarterBalanced.SampleItems.Core.Diagnostics
{
    public class DiagnosticManager : IDiagnosticManager
    {
        private readonly SampleItemsContext context;
        private readonly ILogger logger;

        public enum StatusRating
        {
            Failed = 0,
            Degraded = 1,
            Warning = 2,
            Recovering = 3,
            Ideal = 4
        }

        public DiagnosticManager(SampleItemsContext sampleContext, ILoggerFactory loggerFactory)
        {
            context = sampleContext;
            logger = loggerFactory.CreateLogger<DiagnosticManager>();
        }

        /// <summary>
        /// Asynchronously accesses all AWS nodes' diagnostic APIs, coalesces 
        /// and serializes them as an XML string. 
        /// </summary>
        /// <param name="level"></param>
        /// <returns>a Task<string> of a serialized DiagnosticRoot object.</returns>
        public async Task<string> GetDiagnosticStatusesAsync(int level)
        {
            DiagnosticRoot diagnosticRoot = new DiagnosticRoot();
            List<string> ipAddresses = new List<string>();
            try
            {
                ipAddresses = await GetAwsClusterIps();
            }
            catch (AmazonServiceException ase)
            {
                string msg = context.AppSettings.ExceptionMessages.ErrorDiagnosticApiAws;
                logger.LogError($"Error Code {ase.ErrorCode} Error Message {ase.Message}");
                diagnosticRoot.AddErrorMessage(msg);
            }
            List<Task<DiagnosticStatus>> awsNodeResultTasks = new List<Task<DiagnosticStatus>>();
            foreach (string ipAddress in ipAddresses)
            {
                awsNodeResultTasks.Add(QueryAwsNode(ipAddress, level));
            }

            try
            {
                Task.WaitAll(awsNodeResultTasks.ToArray());
                diagnosticRoot.DiagnosticStatuses = awsNodeResultTasks.Select(t => t.Result).ToList();
            }
            catch (Exception e)
            {
                string msg = context.AppSettings.ExceptionMessages.ErrorDiagnosticApiAwsNode;
                logger.LogError(e.Message);
                diagnosticRoot.AddErrorMessage(msg);
            }

            return Serialize(diagnosticRoot);
        }

        /// <summary>
        /// Asynchronously accesses the local machine's diagnostic information
        /// and serializes it as an XML string.
        /// </summary>
        /// <param name="level"></param>
        /// <returns>a Task<string> of a serialized DiagnosticStatus object.</returns>
        public async Task<string> GetDiagnosticStatusAsync(int level)
        {
            DiagnosticStatus diagnosticStatus = await GetDiagnosticStatusObjectAsync(level);
            return Serialize(diagnosticStatus);
        }

        /// <summary>
        /// Gets a list of AWS cluster ips based on cluster name
        /// </summary>
        /// <returns></returns>
        private async Task<List<string>> GetAwsClusterIps()
        {
            var arns = await GetAwsClusterInstancesArns();

            var clusterIds = await GetAwsClusterInstancesIds(arns);
            var ips = await GetAwsArnsIps(clusterIds);
            return ips;
        }

        /// <summary>
        /// Gets cluster instances AWS arns
        /// </summary>
        /// <returns></returns>
        private async Task<List<string>> GetAwsClusterInstancesArns()
        {
            string awsRegion = context.AppSettings.SettingsConfig.AwsRegion;
            string awsCluster = context.AppSettings.SettingsConfig.AwsClusterName;
            AmazonECSClient ecs = new AmazonECSClient(Amazon.RegionEndpoint.GetBySystemName(awsRegion));

            ecsModel.ListContainerInstancesRequest clusterRequest = new ecsModel.ListContainerInstancesRequest();
            clusterRequest.Cluster = awsCluster;
            var items = await ecs.ListContainerInstancesAsync(clusterRequest);

            return items.ContainerInstanceArns;
        }

        /// <summary>
        /// Gets a list of instance ids from a cluster and instance arns
        /// </summary>
        /// <param name="arns"></param>
        /// <returns></returns>
        private async Task<List<string>> GetAwsClusterInstancesIds(List<string> arns)
        {
            string awsRegion = context.AppSettings.SettingsConfig.AwsRegion;
            string awsCluster = context.AppSettings.SettingsConfig.AwsClusterName;
            AmazonECSClient ecs = new AmazonECSClient(Amazon.RegionEndpoint.GetBySystemName(awsRegion));

            ecsModel.DescribeContainerInstancesRequest clusterRequest = new ecsModel.DescribeContainerInstancesRequest();
            clusterRequest.Cluster = awsCluster;
            clusterRequest.ContainerInstances = arns;
            var items = await ecs.DescribeContainerInstancesAsync(clusterRequest);

            return items.ContainerInstances.Where(t => t.RunningTasksCount > 0).Select(t => t.Ec2InstanceId).ToList();

        }

        /// <summary>
        /// Gets a list of Instances Ips from list of AWS instance ids
        /// </summary>
        /// <param name="instancesIds"></param>
        /// <returns></returns>
        private async Task<List<string>> GetAwsArnsIps(List<string> instancesIds)
        {
            string awsRegion = context.AppSettings.SettingsConfig.AwsRegion;
            AmazonEC2Client ec2 = new AmazonEC2Client(Amazon.RegionEndpoint.GetBySystemName(awsRegion));

            DescribeInstancesRequest request = new DescribeInstancesRequest();
            request.InstanceIds = instancesIds;
            var result = await ec2.DescribeInstancesAsync(request);

            return result.Reservations.SelectMany(t => t.Instances.Where(i => !string.IsNullOrEmpty(i.PublicIpAddress))
                                                    .Select(i => i.PublicIpAddress)).ToList();
        }

        /// <summary>
        /// Queries each AWS node for local diagnostic status xml string.
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="level"></param>
        /// <returns>a Task<DiagnosticStatus>.</returns>
        private async Task<DiagnosticStatus> QueryAwsNode(string ipAddress, int level)
        {
            string statusUrl = context.AppSettings.SettingsConfig.StatusUrl;
            string uri = $"http://{ipAddress}/{statusUrl}{level}";
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(uri);
            XmlSerializer serializer = new XmlSerializer(typeof(DiagnosticStatus));
            var stream = await response.Content.ReadAsStreamAsync();
            DiagnosticStatus status = (DiagnosticStatus)serializer.Deserialize(stream);

            return status;
        }

        /// <summary>
        /// Asynchronously accesses the local machine's diagnostic information.
        /// </summary>
        /// <param name="level"></param>
        /// <returns>a DiagnosticStatus Task.</returns>
        private async Task<DiagnosticStatus> GetDiagnosticStatusObjectAsync(int level)
        {
            Task<SystemStatus> systemStatus = GetSystemStatusAsync(level);
            Task<ConfigurationStatus> configurationStatus = GetConfigurationStatusAsync(level);
            Task<DependencyStatus> depenedencyStatusTask = GetDependencyStatusAsync(level);
            Task<AccessStatus> accessStatusTask = GetAccessStatusAsync(level);

            var diagnosticStatus = new DiagnosticStatus
            {
                Level = level,
                Unit = "webserver",
                CreationTime = DateTime.UtcNow,
                AccessStatus = await accessStatusTask,
                SystemStatus = await systemStatus,
                ConfigurationStatus = await configurationStatus,
                DependencyStatus = await depenedencyStatusTask
            };

            List<int?> childStatusRatings = new List<int?>()
            {
             diagnosticStatus.SystemStatus?.StatusRating,
             diagnosticStatus.AccessStatus?.StatusRating,
             diagnosticStatus.ConfigurationStatus?.StatusRating,
             diagnosticStatus.DependencyStatus?.StatusRating
            };

            int? minStatus = childStatusRatings.Where(t => t.HasValue).Min();
            if (minStatus.HasValue)
            {
                diagnosticStatus.StatusRating = minStatus.Value;
            }

            return diagnosticStatus;
        }

        /// <summary>
        /// Serializes an object with XML Serialization attributes 
        /// as an XML string.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>an XML string.</returns>
        public static string Serialize<T>(T value)
        {
            if (value == null)
            {
                return null;
            }

            XmlSerializer serializer = new XmlSerializer(typeof(T));

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = false;
            settings.OmitXmlDeclaration = true;

            var xns = new XmlSerializerNamespaces();
            xns.Add(string.Empty, string.Empty);

            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings))
                {
                    serializer.Serialize(xmlWriter, value, xns);
                }
                return textWriter.ToString();
            }
        }

        /// <summary>
        /// Formats bytes returned by drive info as "x.x GiB".
        /// </summary>
        /// <returns>a string.</returns>
        private string FormatBytes(float bytes)
        {
            var gb = (bytes / (Math.Pow(1024, 3)));
            return string.Format("{0:F1} GiB", gb);
        }

        /// <summary>
        /// Maps a DriveInfo object onto a FilesystemStatus object.
        /// </summary>
        /// <param name="driveInfo"></param>
        /// <returns>a FilesystemStatus object.</returns>
        private FilesystemStatus GetFilesystemStatus(DriveInfo driveInfo)
        {
            FilesystemStatus fsStatus = new FilesystemStatus();
            fsStatus.StatusRating = (int)StatusRating.Ideal;
            fsStatus.FilesystemType = driveInfo.DriveType.ToString();

            float freeSpace = driveInfo.AvailableFreeSpace;
            float totalSize = driveInfo.TotalSize;
            float percentFreeSpace = (freeSpace / totalSize) * 100;

            fsStatus.FreeSpace = FormatBytes(freeSpace);
            fsStatus.TotalSpace = FormatBytes(totalSize);
            fsStatus.MountPoint = driveInfo.RootDirectory.ToString();
            fsStatus.PercentFreeSpace = percentFreeSpace.ToString();

            if (percentFreeSpace < 5)
            {
                fsStatus.ErrorMessage = $"Less than 5% of disk space remaining on volume \"{driveInfo.VolumeLabel}\"";
                fsStatus.StatusRating = (int)StatusRating.Warning;
            }

            return fsStatus;
        }

        /// <summary>
        /// Assesses the filesystem statuses for the local machine.
        /// </summary>
        /// <returns>a list of FilesystemStatus objects</returns>
        private List<FilesystemStatus> GetFilesystemStatuses()
        {
            var filesystemStatuses = new List<FilesystemStatus>();
            var drives = DriveInfo.GetDrives().Where(t => t.TotalSize > 0.000001).ToList();
            foreach (DriveInfo drive in drives)
            {
                filesystemStatuses.Add(GetFilesystemStatus(drive));
            }

            return filesystemStatuses;
        }

        /// <summary>
        /// Collects the FilesystemStatuses and their errors and warnings
        /// </summary>
        /// <returns>a SystemStatus object.</returns>
        private SystemStatus GetSystemStatus()
        {
            var systemStatus = new SystemStatus();
            systemStatus.FileSystems = GetFilesystemStatuses();
            systemStatus.StatusRating = systemStatus.FileSystems.Min(t => t.StatusRating);

            return systemStatus;
        }

        /// <summary>
        /// Asynchronously collects the FilesystemStatuses and their errors and warnings
        /// if given the appropriate level
        /// </summary>
        /// <param name="level"></param>
        /// <returns>a Task<SystemStatus></returns>
        private Task<SystemStatus> GetSystemStatusAsync(int level)
        {
            return Task.Run(() =>
            {
                return (level >= 1) ? GetSystemStatus() : null;
            });
        }

        /// <summary>
        /// Checks S3 bucket and the AWS-region configurations.
        /// </summary>
        /// <returns>a ConfigurationStatus object.</returns>
        private ConfigurationStatus GetConfigurationStatus()
        {
            ConfigurationStatus configurationStatus = new ConfigurationStatus
            {
                ContentBucket = context.AppSettings.SettingsConfig.AwsS3Bucket,
                AWSRegion = context.AppSettings.SettingsConfig.AwsRegion
            };

            if (string.IsNullOrEmpty(configurationStatus.ContentBucket))
            {
                configurationStatus.AddErrorMessage("Content bucket configuration not found.");
                configurationStatus.StatusRating = (int)StatusRating.Failed;
            }
            if (string.IsNullOrEmpty(configurationStatus.AWSRegion))
            {
                configurationStatus.AddErrorMessage("AWS region configuration not found.");
                configurationStatus.StatusRating = (int)StatusRating.Failed;
            }

            return configurationStatus;
        }

        /// <summary>
        /// Asynchronously checks access to the S3 bucket and the AWS-region.
        /// </summary>
        /// <param name="level"></param>
        /// <returns>a Task<ConfigurationStatus>.</returns>
        private Task<ConfigurationStatus> GetConfigurationStatusAsync(int level)
        {
            return Task.Run(() =>
            {
                return (level >= 2) ? GetConfigurationStatus() : null;
            });
        }

        /// <summary>
        /// Check that data can be read from a local file.
        /// </summary>
        /// <returns>an AccessStatus object.</returns>
        private void GetReadStatus(AccessStatus accessStatus)
        {
            string filepath = context.AppSettings.SettingsConfig.ContentRootDirectory;

            accessStatus.Readable = false;
            try
            {
                Directory.GetFiles(filepath);
                accessStatus.Readable = true;
            }
            catch (UnauthorizedAccessException)
            {
                accessStatus.AddErrorMessage("The caller does not have the required permissions to read the content package.");
                accessStatus.StatusRating = (int)StatusRating.Failed;
            }
            catch (Exception)
            {
                accessStatus.AddErrorMessage("An unknown error occurred while creating a file.");
                accessStatus.StatusRating = (int)StatusRating.Failed;
            }
        }

        /// <summary>
        /// Check that a file can be written to the local filesystem.
        /// </summary>
        /// <returns>an AccessStatus object.</returns>
        private void GetWriteStatus(AccessStatus accessStatus)
        {
            string filepath = context.AppSettings.SettingsConfig.ContentRootDirectory;

            filepath = Path.Combine(filepath, Path.GetRandomFileName());
            while (File.Exists(filepath))
            {
                filepath = Path.Combine(filepath, Path.GetRandomFileName());
            }

            accessStatus.Writable = false;
            try
            {
                File.WriteAllLines(filepath, new string[] { "some random text to write to a file" });
                File.Delete(filepath);
                accessStatus.Writable = true;
            }
            catch (UnauthorizedAccessException)
            {
                accessStatus.AddErrorMessage("The caller does not have the required permissions to create a file.");
                accessStatus.StatusRating = (int)StatusRating.Failed;
            }
            catch (DirectoryNotFoundException)
            {
                accessStatus.AddErrorMessage("The specified path is invalid.");
                accessStatus.StatusRating = (int)StatusRating.Failed;
            }
            catch (IOException)
            {
                accessStatus.AddErrorMessage("An I/O error occurred while creating a file.");
                accessStatus.StatusRating = (int)StatusRating.Failed;
            }
            catch (Exception)
            {
                accessStatus.AddErrorMessage("An unknown error occurred while creating a file.");
                accessStatus.StatusRating = (int)StatusRating.Failed;
            }
        }

        /// <summary>
        /// Level 3 and up checks database (content package) access, 
        /// level 4 and up checks ability to write to a file.
        /// </summary>
        /// <param name="level"></param>
        /// <returns>an AccessStatus object or null.</returns>
        private Task<AccessStatus> GetAccessStatusAsync(int level)
        {
            return Task.Run(() =>
            {
                AccessStatus accessStatus = null;
                if (level >= 3)
                {
                    accessStatus = new AccessStatus();
                    GetReadStatus(accessStatus);

                    if (level >= 4)
                    {
                        GetWriteStatus(accessStatus);
                    }
                }

                return accessStatus;
            });
        }

        /// <summary>
        /// Asynchronously tests access to itemviewerservice.
        /// </summary>
        /// <returns>a DependencyStatus Task or null.</returns>
        private async Task<DependencyStatus> GetDependencyStatusAsync()
        {
            DependencyStatus dependencyStatus = new DependencyStatus();

            string url = context.AppSettings.SettingsConfig.ItemViewerServiceURL;

            HttpClient client = new HttpClient();
            HttpResponseMessage response;
            try
            {
                response = await client.GetAsync(url);
                dependencyStatus.ItemViewerServiceStatus = ((int)response.StatusCode).ToString();
            }
            catch (WebException webExc)
            {
                int statusCode = (int)((HttpWebResponse)webExc.Response).StatusCode;
                dependencyStatus.ItemViewerServiceStatus = statusCode.ToString();
                dependencyStatus.AddErrorMessage("Unable to access ItemViewerService");
                dependencyStatus.StatusRating = (int)StatusRating.Failed;
            }

            return dependencyStatus;
        }

        /// <summary>
        /// Asynchronously tests access to itemviewerservice.
        /// </summary>
        /// <param name="level"></param>
        /// <returns>a DependencyStatus Task or null.</returns>
        private async Task<DependencyStatus> GetDependencyStatusAsync(int level)
        {
            return (level >= 5) ? await GetDependencyStatusAsync() : null;
        }
    }
}
