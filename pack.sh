#!/usr/bin/bash
set -ex
rm -rf nuget
dotnet pack -o ./nuget/
rm ./nuget/DTLib.Tests.*
ls nuget
