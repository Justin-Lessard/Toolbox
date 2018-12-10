makecert.exe ^
-n "CN=%%%---CERTIFICATE---NAME---%%%" ^
-iv CARoot.pvk ^
-ic CARoot.cer ^
-pe ^
-a sha512 ^
-len 4096 ^
-b 01/01/2014 ^
-e 01/01/2040 ^
-sky exchange ^
-eku 1.3.6.1.5.5.7.3.1 ^
-sv ServerCert.pvk ^
ServerCert.cer
pvk2pfx.exe ^
-pvk ServerCert.pvk ^
-spc ServerCert.cer ^
-pfx ServerCert.pfx ^
-po %%%---PASSWORD---PASSWORD---%%% 