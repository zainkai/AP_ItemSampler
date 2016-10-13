using SmarterBalanced.SampleItems.Dal.Context;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Amazon.S3;
using Amazon.S3.Model;
using SmarterBalanced.SampleItems.Dal.Models.Configurations;
using System.IO.Compression;

namespace SmarterBalanced.SampleItems.Dal.Infrastructure
{
    public class ContentDownloader
    {
        private AppSettings settings;
        private string contentDir;

        public ContentDownloader(AppSettings appSettings)
        {
            settings = appSettings;
            contentDir = settings.SettingsConfig.ContentRootDirectory;
        }

        /// <summary>
        /// Downloads a content zip file from S3 using the bucket and key specified in the appsettings
        /// </summary>
        /// <returns></returns>
        private async Task DownloadContentAsync()
        {
            string awsRegion = settings.SettingsConfig.AwsRegion;
            string s3Bucket = settings.SettingsConfig.AwsS3Bucket;
            string zipFileName = settings.SettingsConfig.AwsS3ContentFilename;
            IAmazonS3 client;
            using (client = new AmazonS3Client(Amazon.RegionEndpoint.GetBySystemName(awsRegion)))
            {
                GetObjectRequest request = new GetObjectRequest
                {
                    BucketName = s3Bucket,
                    Key = zipFileName
                };

                using (GetObjectResponse response = await client.GetObjectAsync(request))
                {
                    CancellationTokenSource cancellationSource = new CancellationTokenSource();
                    await response.WriteResponseStreamToFileAsync(contentDir + "/" + zipFileName, false, cancellationSource.Token);
                }
            }
        }

        /// <summary>
        /// Removes "Item" and "Stimuli" directories and their contents,
        /// and the "imsmanifest.xml" from the content directory. 
        /// </summary>
        private Task CleanDirectoryAsync()
        {
            return Task.Run(() =>
            {
                if (Directory.Exists(contentDir))
                {
                    //Remove old content items
                    if (Directory.Exists(contentDir + "/Items"))
                    {
                        Directory.Delete(contentDir + "/Items", true);
                    }

                    //Remove old content stimulus
                    if (Directory.Exists(contentDir + "/Stimuli"))
                    {
                        Directory.Delete(contentDir + "/Stimuli", true);
                    }

                    //Remove old content manifest
                    if (File.Exists(contentDir + "/imsmanifest.xml"))
                    {
                        File.Delete(contentDir + "/imsmanifest.xml");
                    }
                }
            });
        }

        private Task UnpackZip()
        {
            string zipFileName = settings.SettingsConfig.AwsS3ContentFilename;
            string localZipFile = contentDir + "/" + zipFileName;
            return Task.Run(() => {
                using (ZipArchive zip = ZipFile.OpenRead(localZipFile))
                {
                    foreach (ZipArchiveEntry entry in zip.Entries)
                    {
                        string localPath = Path.Combine(contentDir, entry.FullName);;
                        if (string.IsNullOrEmpty(entry.Name))
                        {
                            Directory.CreateDirectory(localPath);
                        }
                        else
                        {
                            entry.ExtractToFile(localPath, true);
                        }
                    }
                }
            });
        }

        /// <summary>
        /// Removes the current content package if it exists
        /// Downloads a new content package
        /// Unpacks the new content package
        /// </summary>
        /// <returns></returns>
        public async Task UpdateContent()
        {
            bool useS3Content = settings.SettingsConfig.UseS3ForContent;
            if (useS3Content)
            {
                Task clean = CleanDirectoryAsync();
                Task fetch = DownloadContentAsync();
                await clean;
                await fetch;
                await UnpackZip();
            }
        }
    }
}
