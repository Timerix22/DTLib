# DtsodC

DtsodV23 parser in C# works too slow, so i wrote V24 parser in C

<br>


## Compiling on Linux
**Required packages:** gcc

Compile with glibc: 
```bash
make std_build
````

Compile with <a href="https://github.com/jart/cosmopolitan">cosmopolitan<a> (crossplatform libc implementation): 
```bash
make build
```

If you see the `run-detectors: unable to find an interpreter` error, just execute this command:
```bash
sudo echo ':APE:M::MZqFpD::/bin/sh:' >/proc/sys/fs/binfmt_misc/register
```