makecert.exe ^
-n "CN=%%%%%api.ad.distributionstox.ca%%%%%" ^
-iv %%%%%STOXIIS3%%%%%.pvk ^
-ic %%%%%STOXIIS3%%%%%.cer ^
-pe ^
-a sha512 ^
-len 4096 ^
-b 01/01/2014 ^
-e 01/01/2040 ^
-sky exchange ^
-eku 1.3.6.1.5.5.7.3.1 ^
-sv %%%%%STOXIIS3%%%%%.api.pvk ^
%%%%%STOXIIS3%%%%%.api.cer

pvk2pfx.exe ^
-pvk %%%%%STOXIIS3.api%%%%%.pvk ^
-spc %%%%%STOXIIS3.api%%%%%.cer ^
-pfx %%%%%STOXIIS3.api%%%%%.pfx ^
-po %%%%%PASSWORD%%%%%PASSWORD%%%%%