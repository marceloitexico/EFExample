"%~dp0packages\OpenCover.4.6.519\tools\OpenCover.Console.exe" ^
-register:user ^
-target:"%VS150COMNTOOLS%\..\IDE\mstest.exe" ^
-targetargs:"/testcontainer:\"%~dp0..\EFCodeFirstTest\bin\Debug\EFCodeFirstTest.dll\" /resultsfile:\"%~dp0EFCodeFirstTest.trx\"" ^
-filter:"+[EFApproaches*]* -[EFCodeFirstTest]* -[*]EFApproaches.RouteConfig" ^
-mergebyhash ^
-skipautoprops ^
-output:"%~dp0\GeneratedReports\EFCodeFirst.xml"
pause
