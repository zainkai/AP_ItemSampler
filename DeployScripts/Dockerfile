FROM alpine:3.3
ARG source=DockerBuild
WORKDIR /app
COPY $source .

RUN apk add --update \
    python \
    unzip \
    curl \
    tar \
    bash
    
RUN curl "https://bootstrap.pypa.io/get-pip.py" -k -o "get-pip.py" && python get-pip.py &&  pip install awscli

RUN curl -fsSLO https://get.docker.com/builds/Linux/x86_64/docker-1.11.2.tgz && tar --strip-components=1 -xvzf docker-1.11.2.tgz -C /usr/local/bin
ENTRYPOINT ["bash", "build.sh"]
