FROM microsoft/aspnetcore-build
ARG source=./SmarterBalanced.SampleItems
WORKDIR /app
COPY $source .

RUN apt-get install -y \
    python

RUN curl "https://bootstrap.pypa.io/get-pip.py" -o "get-pip.py" && python get-pip.py &&  pip install awscli

ENTRYPOINT ["sh", "./build.sh"]
