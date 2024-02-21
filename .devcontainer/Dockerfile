FROM mcr.microsoft.com/dotnet/sdk:8.0

# install need application
RUN apt-get update && \
    apt-get install -y \
    apt-utils \
    locales \
    locales-all \
    sudo \
    unzip \
    vim \
    jq \
    && apt-get clean

# install dotnet tools
# https://learn.microsoft.com/ja-jp/ef/core/cli/dotnet
# https://learn.microsoft.com/ja-jp/aspnet/core/fundamentals/tools/dotnet-aspnet-codegenerator?view=aspnetcore-7.0
RUN dotnet tool install --global dotnet-ef --version 8.0.0 && \
    dotnet tool install --global dotnet-aspnet-codegenerator --version 8.0.0 && \
    dotnet tool restore && \
    dotnet dev-certs https
ENV PATH="${PATH}:/root/.dotnet/tools"

# set locales
RUN locale-gen
RUN update-locale
RUN sed -i s/'# ja_JP.UTF-8 UTF-8'/'ja_JP.UTF-8 UTF-8'/ /etc/locale.gen
ENV LANG="ja_JP.UTF-8"

# install git utility
RUN wget https://raw.githubusercontent.com/git/git/master/contrib/completion/git-completion.bash -O ~/.git-completion.bash
RUN chmod a+x ~/.git-completion.bash
RUN wget https://raw.githubusercontent.com/git/git/master/contrib/completion/git-prompt.sh -O ~/.git-prompt.sh
RUN chmod a+x ~/.git-prompt.sh

# set default ps1 string
RUN { \
echo "source $HOME/.git-completion.bash"; \
echo "source $HOME/.git-prompt.sh"; \
echo "PS1='\${debian_chroot:+(\$debian_chroot)}\\[\\\033[01;32m\\]\\u@\\h\\[\\\033[00m\\]:\\[\\\033[01;34m\\]\\W\\[\\\033[1;30m\\]\$(__git_ps1 \"(%s)\")\\[\\\033[00m\\]\\$ '"; \
     # base ps1 string
     # PS1='${debian_chroot:+($debian_chroot)}\[\033[01;32m\]\u@\h\[\033[00m\]:\[\033[01;34m\]\W\[\033[1;30m\]$(__git_ps1 "(%s)")\[\033[00m\]\$ '
} >> ~/.bashrc

# set git config
RUN git config --global core.quotepath false

# create workdir
RUN mkdir /app
WORKDIR /app

# set label
LABEL Name="mirage"
LABEL Version="0.1"
LABEL Release="0.1"
LABEL Architecuture="amd64"
LABEL Vendor="Creationline.inc"

# set expose
EXPOSE 80
EXPOSE 443

# set cmd
CMD /bin/bash