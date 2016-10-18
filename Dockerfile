FROM microsoft/aspnetcore-build
RUN apt-get install -y \
    python

RUN curl "https://bootstrap.pypa.io/get-pip.py" -o "get-pip.py" && python get-pip.py &&  pip install awscli