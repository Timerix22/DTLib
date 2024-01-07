#!/usr/bin/bash
set -xe

if [[ -d nuget ]]; then
    mkdir -p nuget_old
    TIMESTAMP=$(date +%Y.%m.%d_%H-%M-%S)
    mv nuget "nuget_$TIMESTAMP"
    tar cvf "nuget_old/nuget_$TIMESTAMP.tar" "nuget_$TIMESTAMP"
    rm -rf "nuget_$TIMESTAMP"
    ls -lh nuget_old
fi
mkdir nuget

function create_package() {
    echo "----------[$1]----------"
    cd "$1" || return 1
    dotnet pack -c Release -o bin/pack_tmp || return 1
    rm -rf bin/pack
    mv bin/pack_tmp bin/pack
    cp bin/pack/* ../nuget/
    cd ..
}

set +x
create_package DTLib
create_package DTLib.Ben.Demystifier
create_package DTLib.Dtsod
create_package DTLib.Logging
create_package DTLib.Logging.Microsoft
create_package DTLib.Network
create_package DTLib.XXHash

ls -lh nuget
