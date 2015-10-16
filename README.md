####Background:

"Two Dots" for iOS is a Unity-based game. This can be determined through examining with otool, IDA, etc. Unity games typically use an easily decompiled .NET Assembly for the application data, but on iOS this is converted to native code. 

* <a href="http://blogs.unity3d.com/2015/05/06/an-introduction-to-ilcpp-internals/">Unity - An introduction to ilcpp internals</a>

The iOS package does not contain the original .NET file. However, the original file can be found as *Assembly-CSharp.dll* in the Android package at *assets/bin/Data/Managed*. For the Android app, this can be edited, repackaged and used for complete control of the game. It can also be useful as a guide when locating functions within the compiled code in the iOS binary... 

Using <a href="https://github.com/0xd4d/dnSpy">dnSpy</a>, it was possible to find the function they use to encrypt and decrypt stored game values (<a href="SimpleAES.cs">SimpleAES.cs</a>). This was converted to Python and used to pwn all levels, etc via the game plist file:
* <a href="2dots_decrypt.py">2dots_decrypt.py</a>
* <a href="2dots_encrypt.py">2dots_encrypt.py</a>



####Example:

*(On iPad)*
```
find /var/mobile/Containers/Data/Application -name "com.weplaydots.twodots.plist"
/var/mobile/Containers/Data/Application/5DADF5E8-EFD9-4EEA-B317-119BE0DD62BF/Library/Preferences/com.weplaydots.twodots.plist
```

Use <a href="http://scw.us/iPhone/plutil/">plutil.pl</a> on Linux or plutil on iOS to dump to text ..

	<key>Eraser Quantity</key>
	<string>yFjAWJAL7C0y9F4OXnJyOA==</string>
	<key>Eraser Quantity_validTimeSpanInHours</key>
	<string>kPqwOrU9eEvvjXeaAT1sKw==</string>

Decrypt...

```
echo "kPqwOrU9eEvvjXeaAT1sKw==" | base64 -d | python ./2dots_decrypt.py 
0


echo "yFjAWJAL7C0y9F4OXnJyOA==" | base64 -d | python ./2dots_decrypt.py 
2
```
Let's make that 100000 :)

```
echo -n "100000" | python ./2dots_encrypt.py 
AkpL9N2cOohzKdUPUIquCQ==
```

*(On iPad)*
```
plutil -key "Eraser Quantity" -value "AkpL9N2cOohzKdUPUIquCQ==" com.weplaydots.twodots.plist
plutil -key "Eraser Quantity_validTimeSpanInHours" -value "AkpL9N2cOohzKdUPUIquCQ==" com.weplaydots.twodots.plist
```

See:
* <a href="screenshots/IMG_0708.PNG"> Screenshot of new amounts</a>

---
	<key>Lives Remaining</key>
	<string>kPqwOrU9eEvvjXeaAT1sKw==</string>
	<key>OpenUDID</key>

```
echo "kPqwOrU9eEvvjXeaAT1sKw==" | base64 -d | python ./2dots_decrypt.py
0
```
Zero lives remaining...let's make that 100...
```
echo -n "100" | python ./2dots_encrypt.py 
kRjbR+T3HtBxxXYaZYe70Q==
```
This value can be updated on iOS using plutil or iFile, etc...


Let's investigate the "PuzzlesData" key...

```
	<key>PuzzlesData</key>
	<string>RqH0FRcwD5BUHYKS3t7TAGb4UqASnXzFT2xrNVWaooChdemdKgnDjbqtX0jNaj5uOqQxUEnX4dJWTIRqWO4NFYrNYQtr02HrhHQ0LOECiUQsDzitbDpijtVVdW00VhGnbWPz/96jLRFpxRdKZL3nts+1nvoisCEpsxau/xmyO7hEun5Wjdu9XjQpEHspCCyeE476O56UC8J4UHm4Kxc6wp29pseXa6iacK99xFwf3xPkiJRf66a/Qo2N20IEBFxQ9nzkAlxY0CSbkU/0wQLKzgyRIkirZiSP/xCV+UEwLLYqbJgQSW5NxY3XhMzN7F21YNThKUKRqfGNJxZN84XM9LUGfvW/Qvzt1b3+gDYURU7uCu0pcpJfQ7eSeL6M1jBAqVRPTLuC3D/V8X7jpsXxKC4raI2TLxon/AH3pR7HGcZzCFfyYAfDS96UmBbcoz3njtWevetwe4cKcuWhWKpkVSRgjCk6XgUS+FDHsQe7fa53ExJxN6OqSfJX/V8FowKN3WFAHjOHZwvJ3cXXp9vj81n1s1VJxqUJ4uzI5otRrk42E4eqz2FWgEbl0FjJYZ08HNkxSq1tM4HXTEW7Glq2zOs9hmCCezaZQ7/DhhBVMrQBT7DQnfcM7f//XiYMaCHjg0VleYw/zc9xUimUMauZbl6XvCzqwoZYhDCdeLC/bpbSNYMd2vZwNNQLcIwo0W7vtRCFGH8oZ9K5bFvOBk7xrv6EcW1EDBZJsk4KyhRglzqq5/Iqu5O+4sV8T4/C9lmH2HCYIfuyOGW5zmQ51hP8blY99QtgeVVzAo034CItTVLRrl5f5f1tM5pWTsKF3r4ZolN1Exoy5iDb/D6D3DY71kIAXgAT2G3zQV+Y7BIzeEfGfPIxxjWTmTkhp1dUI/kzSOkySYxpmpGMBuR3EvPCKw==</string>

```
Decrypt...
```
echo "RqH0FRcwD5BUHYKS3t7TAGb4UqASnXzFT2xrNVWaooChdemdKgnDjbqtX0jNaj5uOqQxUEnX4dJWTIRqWO4NFYrNYQtr02HrhHQ0LOECiUQsDzitbDpijtVVdW00VhGnbWPz/96jLRFpxRdKZL3nts+1nvoisCEpsxau/xmyO7hEun5Wjdu9XjQpEHspCCyeE476O56UC8J4UHm4Kxc6wp29pseXa6iacK99xFwf3xPkiJRf66a/Qo2N20IEBFxQ9nzkAlxY0CSbkU/0wQLKzgyRIkirZiSP/xCV+UEwLLYqbJgQSW5NxY3XhMzN7F21YNThKUKRqfGNJxZN84XM9LUGfvW/Qvzt1b3+gDYURU7uCu0pcpJfQ7eSeL6M1jBAqVRPTLuC3D/V8X7jpsXxKC4raI2TLxon/AH3pR7HGcZzCFfyYAfDS96UmBbcoz3njtWevetwe4cKcuWhWKpkVSRgjCk6XgUS+FDHsQe7fa53ExJxN6OqSfJX/V8FowKN3WFAHjOHZwvJ3cXXp9vj81n1s1VJxqUJ4uzI5otRrk42E4eqz2FWgEbl0FjJYZ08HNkxSq1tM4HXTEW7Glq2zOs9hmCCezaZQ7/DhhBVMrQBT7DQnfcM7f//XiYMaCHjg0VleYw/zc9xUimUMauZbl6XvCzqwoZYhDCdeLC/bpbSNYMd2vZwNNQLcIwo0W7vtRCFGH8oZ9K5bFvOBk7xrv6EcW1EDBZJsk4KyhRglzqq5/Iqu5O+4sV8T4/C9lmH2HCYIfuyOGW5zmQ51hP8blY99QtgeVVzAo034CItTVLRrl5f5f1tM5pWTsKF3r4ZolN1Exoy5iDb/D6D3DY71kIAXgAT2G3zQV+Y7BIzeEfGfPIxxjWTmTkhp1dUI/kzSOkySYxpmpGMBuR3EvPCKw==" | base64 -d | python ./2dots_decrypt.py 


{"1":{"score":82, "stars":1}, "2":{"score":150, "stars":2}, "3":{"score":289, "stars":3}, "4":{"score":328, "stars":3}, "5":{"score":280, "stars":3}, "6":{"score":326, "stars":3}, "7":{"score":472, "stars":3}, "8":{"score":181, "stars":3}, "9":{"score":444, "stars":3}, "10":{"score":239, "stars":2}, "11":{"score":269, "stars":3}, "12":{"score":174, "stars":3}, "13":{"score":230, "stars":3}, "14":{"score":320, "stars":3}, "15":{"score":228, "stars":3}, "16":{"score":234, "stars":3}, "17":{"score":435, "stars":3}, "18":{"score":299, "stars":3}, "19":{"score":445, "stars":3}, "20":{"score":418, "stars":3}, "21":{"score":651, "stars":3}, "22":{"score":107, "stars":3}}

```
So, it looks like level, star, and score data...the top level is 460. Let's get 1,000,000 points for all the levels :)

```
 ( echo -n -e "{\"1\":{\"score\":1000000, \"stars\":3},"; for x in `seq 2 459`; do echo -n -e " \"$x\"{:\"score\":1000000, \"stars\":3},";done; echo -e "\"460\":{\"score\":1000000, \"stars\":3}}" ) | python 2dots_encrypt.py

XT5i8DV9WmCdvklOopzTmmT9PaJ2cOlBOJoyjj6rxCJ6un9Ak/eSlWPZ2kgDG6kophqaQwqaHwiOY7W+Ko0JyH9xXQMCYAkDkdjBDbexk8qTZMgvox7vZRExoEiE7ig/gTvzlMqo8t0JWPsAATuBOVtEYmcb8CfK+Ekty7qOTMonse7MQwfHkoTD9I0qj/WfdoMUDrKyTsNJOQMIr4CJApiSNsyOPP3KRPhttPDO1dgGhgWZqgEJgmwCPelXwTQXzeVhzeW ...
```

This is a fairly large value, so I used scp to copy a text file of the value to my device and then used plutil to change it. Worked like a charm! 

See:
* <a href="screenshots/IMG_0706.PNG"> Screenshot of all levels unlocked</a>
* <a href="screenshots/IMG_0707.PNG"> Screenshot of 1,000,000 points</a>

####Update - C# Version:
Using Mono to run existing SimpleAES.cs:

See:
* <a href="SimpleAES_encrypt.cs">SimpleAES_encrypt.cs</a>
* <a href="SimpleAES_decrypt.cs">SimpleAES_decrypt.cs</a>

Example:
```
echo "nSquSSl4Zqndiih+kRqmwPCcL2JoKuH13+VsNJlX2UQ=" | mono SimpleAES_decrypt.exe | mono SimpleAES_encrypt.exe
nSquSSl4Zqndiih+kRqmwPCcL2JoKuH13+VsNJlX2UQ=
```



####Developers: 

Know that any keys, passwords, etc that are compiled into your software are easily discovered through basic reverse engineering.







