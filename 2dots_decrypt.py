import pyaes
import sys

key = str(bytearray([12,128,45,11,24,26,14,6,12,184,4,162,37,112,18,209]))

iv = str(bytearray([146,12,6,111,4,2,101,45,97,121,18,14,79,32,114,156]))

decrypter = pyaes.Decrypter(pyaes.AESModeOfOperationCBC(key, iv))

decrypted = ""

for line in sys.stdin:
   decrypted += decrypter.feed(line)

decrypted += decrypter.feed()

print decrypted


