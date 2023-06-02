# ITN_Data_Downloader
Itnetwork Data Downloader is a C# console application used to download public data about users of www.itnetwork.cz. <br>
After downloading, the data is stored in an MS-SQL database.<br>
The program downloads an HTML string (the code of the given page) in the specified range, searches for rows with
the relevant properties, stores them in the relevant properties of the User class and the collection of downloaded
The downloaded users are stored in the internal collection.<br>
When the download is complete, it is saved to the database using ADO.NET.<br>
The database must be in the program folder for the program to run correctly.<br>
The data is further used in the <a href="https://github.com/TomasSobotaT/ITN_Data_Presenter"> ITN Data Presenter </a> project.<br>

Documentation (CZ): <a href="https://tsobota.cz/C_ITNDataDownloader.html"> tsobota.cz/C_ITNDataDownloader.html<a>
