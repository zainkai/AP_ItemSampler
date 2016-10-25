FROM debian
ARG source=DockerBuild
WORKDIR /app
COPY $source .

RUN apt-get update -qq && apt-get install -qqy \
    python \
    unzip \
    curl
    
RUN curl "https://bootstrap.pypa.io/get-pip.py" -o "get-pip.py" && python get-pip.py &&  pip install awscli

RUN curl -fsSLO https://get.docker.com/builds/Linux/x86_64/docker-1.11.2.tgz && tar --strip-components=1 -xvzf docker-1.11.2.tgz -C /usr/local/bin
ENTRYPOINT ["bash", "build.sh"]