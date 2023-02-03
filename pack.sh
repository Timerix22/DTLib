#!/usr/bin/bash
set -ex
dotnet pack -c Release -o ./nuget_new/
rm ./nuget_new/DTLib.Tests.*
rm -rf nuget
mv nuget_new nuget
ls nuget
