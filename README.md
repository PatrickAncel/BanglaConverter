# Bangla Converter

This program is to help those used to English keyboards type Bangla characters. It converts key presses and key combinations into their Bangla equivalents. For example, <kbd>K</kbd> produces ক, and <kbd>Shift</kbd> + <kbd>K</kbd> produces the aspirated version: খ. This tool is still being developed. Come back later for more features.

In the future, this program will allow mapping custom keys and key combinations to specific characters. For now, the tables below show the keys and key combinations required to type each Bangla character.

এই প্রোগ্রামটা তাদের জন্য যারা ইংরেজি কিবোর্ডে বাংলা লিখে অভ্যস্ত | এটা কিবোর্ডের কী প্রেস ইনপুট নেয় এবং সেটাকে বাংলা অক্ষরে রুপান্তরিত করে | যেমন "K" অক্ষরটি কিবোর্ডে চাপ দিলে এটা "ক" এবং "Shift + K" চাপলে "খ" লিখবে | এই প্রোগ্রামের কাজ এখনও চলমান | নতুন আপডেট এবং ফিচার সম্পর্কে জানতে নিয়মিত চোখ রাখুন |
পরবর্তীতে এই প্রোগ্রাম "কাস্টম কী ম্যাপিং"  এবং নির্দিষ্ট অক্ষরের "কী কম্বিনেশনে" সহায়তা করবে | সুবিধার জন্য নিচে একটি টেবিলে প্রতিটি বাংলা অক্ষরের "কী" এবং "কী কম্বিনেশন" দেওয়া হল |

## Modes

The converter has two modes: English mode and Bangla mode. While in English mode, the program will not convert key presses into Bangla characters. While in Bangla mode, it will convert keys and key combinations to Bangla characters according to the tables below. To toggle between these modes, press <kbd>F1</kbd>.

There are two modes within Bangla mode: full-vowel mode and vowel-sign mode. While in full-vowel mode, the converter will convert appropriate keys and key combinations to full vowels. While in vowel-sign mode, it will convert them to vowel signs. The same key or key combination is used for a full vowel and the corresponding vowel sign. While in Bangla mode, press <kbd>&#47;</kbd> to toggle between full-vowel and vowel-sign mode. The converter will automatically toggle between these modes as you type based on what is to the left of the cursor, but you will still need to toggle manually with <kbd>&#47;</kbd> to write words like বই, because the converter is expecting a vowel sign to follow ব rather than a full vowel.

## Vowels

|         |         |         |         |         |         |
| :-----: | :-----: | :-----: | :-----: | :-----: | :-----: |
| &#2437; | &#2438; | &#2439; | &#2440; | &#2441; | &#2442; |
| <kbd>Shift</kbd>&nbsp;+&nbsp;<kbd>A</kbd> | <kbd>A</kbd> | <kbd>I</kbd> | <kbd>Shift</kbd>&nbsp;+&nbsp;<kbd>I</kbd> | <kbd>U</kbd> | <kbd>Shift</kbd>&nbsp;+&nbsp;<kbd>U</kbd> |

|         |         |         |         |         |
| :-----: | :-----: | :-----: | :-----: | :-----: |
| &#2443; | &#2447; | &#2448; | &#2451; | &#2452; |
| <kbd>Shift</kbd>&nbsp;+&nbsp;<kbd>R</kbd> | <kbd>E</kbd> | <kbd>Shift</kbd>&nbsp;+&nbsp;<kbd>E</kbd> | <kbd>O</kbd> | <kbd>Shift</kbd>&nbsp;+&nbsp;<kbd>O</kbd> |

## Diacritics

|         |         |         |         |         |         |
| :-----: | :-----: | :-----: | :-----: | :-----: | :-----: |
| &#2434; | &#2435; | &#2433; | &#2492; | &#2509; | &#2404; | 
| <kbd>;</kbd> | <kbd>Shift</kbd>&nbsp;+&nbsp;<kbd>;</kbd> | <kbd>`</kbd> | <kbd>.</kbd> | <kbd>,</kbd> | <kbd>&bsol;</kbd> | 
| semicolon | shift + semicolon | backtick | period | comma | backslash |


Most of the diacritics are assigned to keys based on their visual similarity to those symbols, not because of the meaning of those symbols. Additionally, the keys shown above are based on the U.S. keyboard layout. The actual key required to type some symbols may differ from what is in the table.

To combine consonants into conjuncts, place a "&#2509;" between them.

## Consonants

|         |         |         |         |         |
| :-----: | :-----: | :-----: | :-----: | :-----: |
| &#2453; | &#2454; | &#2455; | &#2456; | &#2457; |
| <kbd>K</kbd> | <kbd>Shift</kbd>&nbsp;+&nbsp;<kbd>K</kbd> | <kbd>G</kbd> | <kbd>Shift</kbd>&nbsp;+&nbsp;<kbd>G</kbd> | <kbd>Ctrl</kbd>&nbsp;+&nbsp;<kbd>N</kbd> |
| &#2458; | &#2459; | &#2460; | &#2461; | &#2462; |
| <kbd>C</kbd> | <kbd>Shift</kbd>&nbsp;+&nbsp;<kbd>C</kbd> | <kbd>J</kbd> | <kbd>Shift</kbd>&nbsp;+&nbsp;<kbd>J</kbd> | <kbd>Ctrl</kbd>&nbsp;+&nbsp;<kbd>Shift</kbd>&nbsp;+&nbsp;<kbd>N</kbd> |
| &#2463; | &#2464; | &#2465; | &#2466; | &#2467; |
| <kbd>Ctrl</kbd>&nbsp;+&nbsp;<kbd>T</kbd> | <kbd>Ctrl</kbd>&nbsp;+&nbsp;<kbd>Shift</kbd>&nbsp;+&nbsp;<kbd>T</kbd> | <kbd>Ctrl</kbd>&nbsp;+&nbsp;<kbd>D</kbd> | <kbd>Ctrl</kbd>&nbsp;+&nbsp;<kbd>Shift</kbd>&nbsp;+&nbsp;<kbd>D</kbd> | <kbd>Ctrl</kbd>&nbsp;+&nbsp;<kbd>N</kbd> |
| &#2468; | &#2469; | &#2470; | &#2471; | &#2472; |
| <kbd>T</kbd> | <kbd>Shift</kbd>&nbsp;+&nbsp;<kbd>T</kbd> | <kbd>D</kbd> | <kbd>Shift</kbd>&nbsp;+&nbsp;<kbd>D</kbd> | <kbd>N</kbd> |
| &#2474; | &#2475; | &#2476; | &#2477; | &#2478; |
| <kbd>P</kbd> | <kbd>Shift</kbd>&nbsp;+&nbsp;<kbd>P</kbd> | <kbd>B</kbd> | <kbd>Shift</kbd>&nbsp;+&nbsp;<kbd>B</kbd> | <kbd>M</kbd> |

|         |         |         |         |         |         |         |
| :-----: | :-----: | :-----: | :-----: | :-----: | :-----: | :-----: |
| &#2479; | &#2480; | &#2482; | &#2486; | &#2487; | &#2488; | &#2489; |
| <kbd>Y</kbd> | <kbd>R</kbd> | <kbd>L</kbd> | <kbd>Shift</kbd>&nbsp;+&nbsp;<kbd>S</kbd> | <kbd>Ctrl</kbd>&nbsp;+&nbsp;<kbd>S</kbd> | <kbd>S</kbd> | <kbd>H</kbd> |

|         |
| :-----: |
| &#2510; |
| <kbd>Alt</kbd>&nbsp;+&nbsp;<kbd>T</kbd> |

The letters &#2465;&#2492;, &#2466;&#2492;, and &#2479;&#2492; can be formed by typing &#2465;, &#2466;, or &#2479;, followed by a &#2492; diacritic.
