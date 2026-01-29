# Unity development environment for C# development
FROM mcr.microsoft.com/dotnet/sdk:8.0

# Install required tools for Unity development
RUN apt-get update && apt-get install -y \
    git \
    wget \
    curl \
    vim \
    nano \
    build-essential \
    && rm -rf /var/lib/apt/lists/*

# Set working directory
WORKDIR /workspace

# Copy project files
COPY . /workspace/

# Default to bash shell
CMD ["/bin/bash"]