makecert.exe ^
-n "CN=%%%%%STOXIIS3%%%%%" ^
-r ^
-pe ^
-a sha512 ^
-len 4096 ^
-cy authority ^
-sv %%%%%STOXIIS3%%%%%.pvk ^
STOXIIS3.cer
pvk2pfx.exe ^
-pvk %%%%%STOXIIS3%%%%%.pvk ^
-spc %%%%%STOXIIS3%%%%%.cer ^
-pfx %%%%%STOXIIS3%%%%%.pfx ^
-po %%%%%PASSWORD%%%%%PASSWORD%%%%%
 