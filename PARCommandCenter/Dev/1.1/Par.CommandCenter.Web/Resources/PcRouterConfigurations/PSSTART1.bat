@rem start parcharge

start /min c:\parex\cowserver.exe CONFIG=c:\parex\commdata\cowserver_1.CFG LOG_FILE=COWSERVER1.1
if exist c:\parex\COWserver\commdata\cowserver_2.CFG goto 30
goto 40
:30
start /min c:\parex\cowserver.exe CONFIG=c:\parex\commdata\cowserver_2.CFG LOG_FILE=COWSERVER2.1

:40
start /min c:\parex\cloudrouter.exe ROUTER_ADDRESS=[%tag5%] CONFIG=c:\parex\commdata\cloudrouter.cfg

start chrome "https://cloud.parexcellence.com/parcharge"
exit
