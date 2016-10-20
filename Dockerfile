FROM docker:latest
WORKDIR /app


RUN apt-get update -qq && apt-get install -qqy \
    python \
    unzip
    
RUN curl "https://bootstrap.pypa.io/get-pip.py" -o "get-pip.py" && python get-pip.py &&  pip install awscli

