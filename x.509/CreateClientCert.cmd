makecert.exe ^
-n "CN=%%%---CERTIFICATE---NAME---%%%" ^
-iv CARoot.pvk ^
-ic CARoot.cer ^
-pe ^
-a sha512 ^
-len 4096 ^
-b 01/01/2018 ^
-e 01/01/2040 ^
-sky exchange ^
-eku 1.3.6.1.5.5.7.3.2 ^
-sv ClientCert.pvk ^
ClientCert.cer
pvk2pfx.exe ^
-pvk ClientCert.pvk ^
-spc ClientCert.cer ^
-pfx ClientCert.pfx ^
-po %%%---PASSWORD---PASSWORD---%%%