#!/bin/bash
dotnet build -c Release
dotnet build -c Debug
rm -rf bin
mkdir bin
cp -rv DTLib.*/bin/* bin/
