FROM ubuntu:18.04

WORKDIR /unity

COPY Builds/Server/ ./

# workaround
# waiting for sidecar is ready
CMD sleep 1 && ./AgonesEchoSampleServer.x86_64