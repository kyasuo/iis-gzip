# iis-gzip

The iis-gzip is a concole application to use gzip.dll of IIS.<br>
The specified file as argument is compressed by level from 0 to 10.

___
## Build
#### Requirement
* .NET Framework 4.0 or later

#### Procedure
1. Clone or download this repository.
2. Run "iis-gzip.bat" file. <br>
  (change "csc.exe" path in "iis-gzip.bat" file as necessary)<br>
=> "dist/iis-gzip.exe" is output.

___
## Usage
#### Requirement
* IIS gzip.dll ("C:\\Windows\\System32\\inetsrv\\gzip.dll")

#### Procedure
    iis-gzip.exe [input file path]

After the execution, the gzip files are output in the same folder of input file.

###### Note
The input file size is limited to 128MB.

___
## License
MIT
