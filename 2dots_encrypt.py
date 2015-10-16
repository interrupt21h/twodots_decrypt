import pyaes
import base64
import sys

key = str(bytearray([12,128,45,11,24,26,14,6,12,184,4,162,37,112,18,209]))

iv = str(bytearray([146,12,6,111,4,2,101,45,97,121,18,14,79,32,114,156]))


ciphertext = ''

encrypter = pyaes.Encrypter(pyaes.AESModeOfOperationCBC(key, iv))
for line in sys.stdin:
    ciphertext += encrypter.feed(line)

ciphertext += encrypter.feed()

print base64.b64encode(ciphertext)

