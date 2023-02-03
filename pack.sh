#!/usr/bin/bash
set -ex
rm -rf nuget
mkdir nuget
dotnet pack -c Release -o ./nuget/
rm ./nuget/DTLib.Tests.*
ls nuget
