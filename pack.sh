#!/usr/bin/bash
set -ex
rm -rf nuget_new
dotnet pack -c Publish -o ./nuget_new/
rm -rf nuget
mv nuget_new nuget
ls nuget
