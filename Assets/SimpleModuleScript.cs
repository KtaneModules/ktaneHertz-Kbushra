using System.Collections.Generic;
using UnityEngine;
using KModkit;
using Newtonsoft.Json;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections;
using System;
using Rnd = UnityEngine.Random;

public class SimpleModuleScript : MonoBehaviour {

	public KMAudio audio;
	public KMBombInfo info;
	public KMBombModule module;
	public KMColorblindMode colorblindModule;
	public TextMesh[] colorblindText;
	public KMSelectable[] messageButtons;
	public KMSelectable[] hertzButtons;
	static int ModuleIdCounter = 1;
	int ModuleId;

	bool _isSolved = false;
	bool incorrect = false;
	bool flashing = false;

	public Material[] colorMat;
	Renderer colorRenderer1;
	Renderer colorRenderer2;
	public GameObject[] colorDisplays;
	int randomColor1;
	int randomColor2;
	public string message;
	private int messageInt;
	private int[] encodedMessage;

	private string[] morseWords = new string[50]
	{
		"kitchen",
		"pepper",
		"top",
		"whip",
		"inspector",
		"secure",
		"chest",
		"copy",
		"porter",
		"shareholder",
		"pause",
		"storm",
		"printer",
		"wheat",
		"remunerate",
		"deport",
		"tire",
		"environment",
		"reconcile",
		"diet",
		"walk",
		"exit",
		"toll",
		"of",
		"landowner",
		"progress",
		"citizen",
		"uniform",
		"stick",
		"indication",
		"security",
		"fleet",
		"experienced",
		"expectation",
		"shelter",
		"handicap",
		"say",
		"contemporary",
		"spoil",
		"cash",
		"graduate",
		"selection",
		"minimum",
		"sailor",
		"contraction",
		"delay",
		"predator",
		"sunshine",
		"prize",
		"fry"
	};

	public Material[] flashMat;
	Renderer flashRenderer1;
	public GameObject flashDisplay;

	public TextMesh inputScreen;

	public int hertzNum = 0;
	private static readonly int[,] modifyTable1 = new int[5, 5] 
	{
		{857, 938, 859, 0, 0},
		{490, 607, 206, 150,0},
		{552, 569, 466, 282, 948},
		{0, 485, 262, 308, 345},
		{0, 0, 398, 423, 332}
	};
	private int modifyTable1Row = 2;
	private int modifyTable1Column = 2;

	private int[] chooserTable = new int[19] {
		150,
		206,
		262,
		282,
		308,
		332,
		345,
		398,
		423,
		466,
		485,
		490,
		552,
		569,
		607,
		857,
		859,
		938,
		948
	};
	public TextMesh hertzChooserText;
	private int hertzChooserNum = -1;

	private static readonly int[,] modifyTable2 = new int[5, 5] 
	{
		{0, 12, 11, 13, 19},
		{9, 8, 6, 5, 2},
		{7, 3, 6, 5, 2},
		{1, 0, 1, 7, 6},
		{4, 9, 4, 3, 8}
	};
	public int modifyTable2MessageInt;

	private string[] alphabet = new string[26] 
	{
		"A",
		"B",
		"C",
		"D",
		"E",
		"F",
		"G",
		"H",
		"I",
		"J",
		"K",
		"L",
		"M",
		"N",
		"O",
		"P",
		"Q",
		"R",
		"S",
		"T",
		"U",
		"V",
		"W",
		"X",
		"Y",
		"Z"
	};
	public string letterSubmitAnswers;
	public int[] intSubmitAnswers;
	public int curChar = 0;

	private bool colorblindSupportOn;

	void Awake() 
	{
		ModuleId = ModuleIdCounter++;

		foreach (KMSelectable button in messageButtons)
		{
			KMSelectable pressedButton = button;
			button.OnInteract += delegate () { buttonsMessage(pressedButton); return false; };
		}

		foreach (KMSelectable button in hertzButtons)
		{
			KMSelectable pressedButton = button;
			button.OnInteract += delegate () { buttonsHertz(pressedButton); return false; };
		}
	}

	void Start ()
	{
		randomColor1 = Rnd.Range (0, 7);
		randomColor2 = Rnd.Range (0, 7);

		colorRenderer1 = colorDisplays[0].GetComponent<Renderer> ();
		colorRenderer1.enabled = true;
		colorRenderer1.sharedMaterial = colorMat [randomColor1];

		colorRenderer2 = colorDisplays[1].GetComponent<Renderer> ();
		colorRenderer2.enabled = true;
		colorRenderer2.sharedMaterial = colorMat [randomColor2];

		colorblindSupportOn = colorblindModule.ColorblindModeActive;

		if (colorblindSupportOn == true) 
		{
			if (colorRenderer1.sharedMaterial == colorMat [0]) 
			{
				colorblindText [0].text = "BK"; 
			}
			if (colorRenderer1.sharedMaterial == colorMat [1]) 
			{
				colorblindText [0].text = "G"; 
			}
			if (colorRenderer1.sharedMaterial == colorMat [2]) 
			{
				colorblindText [0].text = "O"; 
			}
			if (colorRenderer1.sharedMaterial == colorMat [3]) 
			{
				colorblindText [0].text = "R"; 
			}
			if (colorRenderer1.sharedMaterial == colorMat [4]) 
			{
				colorblindText [0].text = "W"; 
			}
			if (colorRenderer1.sharedMaterial == colorMat [5]) 
			{
				colorblindText [0].text = "BE"; 
			}
			if (colorRenderer1.sharedMaterial == colorMat [6]) 
			{
				colorblindText [0].text = "P"; 
			}

			if (colorRenderer2.sharedMaterial == colorMat [0]) 
			{
				colorblindText [2].text = "BK"; 
			}
			if (colorRenderer2.sharedMaterial == colorMat [1]) 
			{
				colorblindText [2].text = "G"; 
			}
			if (colorRenderer2.sharedMaterial == colorMat [2]) 
			{
				colorblindText [2].text = "O"; 
			}
			if (colorRenderer2.sharedMaterial == colorMat [3]) 
			{
				colorblindText [2].text = "R"; 
			}
			if (colorRenderer2.sharedMaterial == colorMat [4]) 
			{
				colorblindText [2].text = "W"; 
			}
			if (colorRenderer2.sharedMaterial == colorMat [5]) 
			{
				colorblindText [2].text = "BE"; 
			}
			if (colorRenderer2.sharedMaterial == colorMat [6]) 
			{
				colorblindText [2].text = "P"; 
			}

			colorblindText[1].text = "ORANGE";
		}
		else
		{
			colorblindText [0].text = "";
			colorblindText [1].text = "";
			colorblindText [2].text = "";
		}

		hertzModifier ();
	}

	void hertzModifier()
	{
		//hertzNum modification
		hertzNum = modifyTable1[2,2];
		modifyTable1Row = 2;
		modifyTable1Column = 2;

		if (randomColor1 == 0) 
		{
			hertzNum = modifyTable1[modifyTable1Row,modifyTable1Column];
		}
		if (randomColor1 == 1) 
		{
			modifyTable1Row = modifyTable1Row - 1;
			if (modifyTable1Row == -1) 
			{
				modifyTable1Row = 4;
			}
			hertzNum = modifyTable1[modifyTable1Row,modifyTable1Column];
		}
		if (randomColor1 == 2) 
		{
			modifyTable1Column = modifyTable1Column + 1;
			if (modifyTable1Column == 5) 
			{
				modifyTable1Column = 0;
			}
			hertzNum = modifyTable1[modifyTable1Row,modifyTable1Column];
		}
		if (randomColor1 == 3) 
		{
			modifyTable1Row = modifyTable1Row + 1;
			if (modifyTable1Row == 5) 
			{
				modifyTable1Row = 0;
			}
			hertzNum = modifyTable1[modifyTable1Row,modifyTable1Column];
		}
		if (randomColor1 == 4) 
		{
			modifyTable1Column = modifyTable1Column - 1;
			if (modifyTable1Column == -1) 
			{
				modifyTable1Column = 4;
			}
			hertzNum = modifyTable1[modifyTable1Row,modifyTable1Column];
		}
		if (randomColor1 == 5) 
		{
			modifyTable1Row = modifyTable1Row - 1;
			if (modifyTable1Row == -1) 
			{
				modifyTable1Row = 4;
			}
			modifyTable1Column = modifyTable1Column + 1;
			if (modifyTable1Column == 5) 
			{
				modifyTable1Column = 0;
			}
			hertzNum = modifyTable1[modifyTable1Row,modifyTable1Column];
		}
		if (randomColor1 == 6) 
		{
			modifyTable1Row = modifyTable1Row - 1;
			if (modifyTable1Row == -1) 
			{
				modifyTable1Row = 4;
			}
			modifyTable1Column = modifyTable1Column - 1;
			if (modifyTable1Column == -1) 
			{
				modifyTable1Column = 4;
			}
			hertzNum = modifyTable1[modifyTable1Row,modifyTable1Column];
		}

		if (randomColor2 == 0) 
		{
			hertzNum = modifyTable1[modifyTable1Row,modifyTable1Column];
		}
		if (randomColor2 == 1) 
		{
			modifyTable1Row = modifyTable1Row - 1;
			if (modifyTable1Row == -1) 
			{
				modifyTable1Row = 4;
			}
			hertzNum = modifyTable1[modifyTable1Row,modifyTable1Column];
		}
		if (randomColor2 == 2) 
		{
			modifyTable1Column = modifyTable1Column + 1;
			if (modifyTable1Column == 5) 
			{
				modifyTable1Column = 0;
			}
			hertzNum = modifyTable1[modifyTable1Row,modifyTable1Column];
		}
		if (randomColor2 == 3) 
		{
			modifyTable1Row = modifyTable1Row + 1;
			if (modifyTable1Row == 5) 
			{
				modifyTable1Row = 0;
			}
			hertzNum = modifyTable1[modifyTable1Row,modifyTable1Column];
		}
		if (randomColor2 == 4) 
		{
			modifyTable1Column = modifyTable1Column - 1;
			if (modifyTable1Column == -1) 
			{
				modifyTable1Column = 4;
			}
			hertzNum = modifyTable1[modifyTable1Row,modifyTable1Column];
		}
		if (randomColor2 == 5) 
		{
			modifyTable1Row = modifyTable1Row - 1;
			if (modifyTable1Row == -1) 
			{
				modifyTable1Row = 4;
			}
			modifyTable1Column = modifyTable1Column + 1;
			if (modifyTable1Column == 5) 
			{
				modifyTable1Column = 0;
			}
			hertzNum = modifyTable1[modifyTable1Row,modifyTable1Column];
		}
		if (randomColor2 == 6) 
		{
			modifyTable1Row = modifyTable1Row - 1;
			if (modifyTable1Row == -1) 
			{
				modifyTable1Row = 4;
			}
			modifyTable1Column = modifyTable1Column - 1;
			if (modifyTable1Column == -1) 
			{
				modifyTable1Column = 4;
			}
			hertzNum = modifyTable1[modifyTable1Row,modifyTable1Column];
		}

		if (info.GetOnIndicators ().ToList ().Count == 0) 
		{
			modifyTable1Row = modifyTable1Row - 2;
			if (modifyTable1Row == -1) 
			{
				modifyTable1Row = 4;
			}
			if (modifyTable1Row == -2) 
			{
				modifyTable1Row = 3;
			}
			hertzNum = modifyTable1[modifyTable1Row,modifyTable1Column];
		}
		if (info.GetOnIndicators ().ToList ().Count == 1) 
		{
			modifyTable1Row = modifyTable1Row - 2;
			if (modifyTable1Row == -1) 
			{
				modifyTable1Row = 4;
			}
			if (modifyTable1Row == -2) 
			{
				modifyTable1Row = 3;
			}
			modifyTable1Column = modifyTable1Column + 2;
			if (modifyTable1Column == 5) 
			{
				modifyTable1Column = 0;
			}
			if (modifyTable1Column == 6) 
			{
				modifyTable1Column = 1;
			}
			hertzNum = modifyTable1[modifyTable1Row,modifyTable1Column];
		}
		if (info.GetOnIndicators ().ToList ().Count == 2) 
		{
			modifyTable1Column = modifyTable1Column + 2;
			if (modifyTable1Column == 5) 
			{
				modifyTable1Column = 0;
			}
			if (modifyTable1Column == 6) 
			{
				modifyTable1Column = 1;
			}
			hertzNum = modifyTable1[modifyTable1Row,modifyTable1Column];
		}
		if (info.GetOnIndicators ().ToList ().Count == 3) 
		{
			modifyTable1Row = modifyTable1Row + 2;
			if (modifyTable1Row == 5) 
			{
				modifyTable1Row = 0;
			}
			if (modifyTable1Row == 6) 
			{
				modifyTable1Row = 1;
			}
			modifyTable1Column = modifyTable1Column + 2;
			if (modifyTable1Column == 5) 
			{
				modifyTable1Column = 0;
			}
			if (modifyTable1Column == 6) 
			{
				modifyTable1Column = 1;
			}
			hertzNum = modifyTable1[modifyTable1Row,modifyTable1Column];
		}
		if (info.GetOnIndicators ().ToList ().Count == 4) 
		{
			modifyTable1Row = modifyTable1Row + 2;
			if (modifyTable1Row == 5) 
			{
				modifyTable1Row = 0;
			}
			if (modifyTable1Row == 6) 
			{
				modifyTable1Row = 1;
			}
			hertzNum = modifyTable1[modifyTable1Row,modifyTable1Column];
		}
		if (info.GetOnIndicators ().ToList ().Count == 5) 
		{
			modifyTable1Column = modifyTable1Column - 2;
			if (modifyTable1Column == -1) 
			{
				modifyTable1Column = 4;
			}
			if (modifyTable1Column == -2) 
			{
				modifyTable1Column = 3;
			}
			hertzNum = modifyTable1[modifyTable1Row,modifyTable1Column];
		}
		if (info.GetOnIndicators ().ToList ().Count > 5) 
		{
			modifyTable1Column = modifyTable1Column - 2;
			if (modifyTable1Column == -1) 
			{
				modifyTable1Column = 4;
			}
			if (modifyTable1Column == -2) 
			{
				modifyTable1Column = 3;
			}
			modifyTable1Row = modifyTable1Row - 2;
			if (modifyTable1Row == -1) 
			{
				modifyTable1Row = 4;
			}
			if (modifyTable1Row == -2) 
			{
				modifyTable1Row = 3;
			}
			hertzNum = modifyTable1[modifyTable1Row,modifyTable1Column];
		}
			
		if (hertzNum == 0) 
		{
			Start ();
		}
		else 
		{
			Debug.LogFormat ("[Hertz #{0}] The hertz number is {1}", ModuleId, hertzNum);
		}
	}

	void messageToInt()
	{
		modifyTable2MessageInt = modifyTable2 [modifyTable1Row, modifyTable1Column];
		modifyTable2MessageInt++;
		modifyTable2MessageInt = modifyTable2MessageInt * (info.GetPortPlateCount () + 1);
		Debug.LogFormat("[Hertz #{0}] Number has become {1}", ModuleId, modifyTable2MessageInt);

		bool vowelFound = false;
		foreach (char c in message)
		{
			if ((c == 'a' || c == 'e' || c == 'i' || c == 'o' || c == 'u') && vowelFound == false) 
			{
				modifyTable2MessageInt = Mathf.Abs ((modifyTable2MessageInt - 3) * 2);
				Debug.LogFormat ("[Hertz #{0}] Vowel modifications, number has now become {1}", ModuleId, modifyTable2MessageInt);
				vowelFound = true;
			}
		}

		bool BCDFGFound = false;
		foreach (char c in message)
		{
			if ((c == 'b' || c == 'c' || c == 'd' || c == 'f' || c == 'g') && BCDFGFound == false) 
			{
				modifyTable2MessageInt = modifyTable2MessageInt + 13;
				BCDFGFound = true;
				Debug.LogFormat ("[Hertz #{0}] BCDFG modifications, number has now become {1}", ModuleId, modifyTable2MessageInt);
			}
		}

		bool LMNPQFound = false;
		foreach (char c in message)
		{
			if ((c == 'l' || c == 'm' || c == 'n' || c == 'p' || c == 'q') && LMNPQFound == false)
			{
				modifyTable2MessageInt = modifyTable2MessageInt / 5;
				LMNPQFound = true;
				Debug.LogFormat ("[Hertz #{0}] LMNPQ modifications, number has now become {1}", ModuleId, modifyTable2MessageInt);
			}
		}

		bool RWXYZFound = false;
		foreach (char c in message) 
		{
			if ((c == 'r' || c == 'w' || c == 'x' || c == 'y' || c == 'z') && RWXYZFound == false)
			{
				modifyTable2MessageInt = (modifyTable2MessageInt + 10) % 76;
				RWXYZFound = true;
				Debug.LogFormat ("[Hertz #{0}] RWXYZ modifications, number has now become {1}", ModuleId, modifyTable2MessageInt);
			}
		}

		letterSubmitAnswers = "";

		modifyTable2MessageInt = modifyTable2MessageInt % 26;
		letterSubmitAnswers = alphabet [modifyTable2MessageInt];

		intSubmitAnswers = Encode (letterSubmitAnswers);
		Debug.LogFormat("[Hertz #{0}] Letter that must be submitted is {1}", ModuleId, letterSubmitAnswers);
	}

	int[] Encode(string s)
	{
		var mc = new List<int>();
		foreach (char c in s.ToUpperInvariant())
		{
			switch (c) 
			{
			case 'A':
				mc.Add (1);
				mc.Add (0);
				mc.Add (1);
				mc.Add (0);
				mc.Add (0);
				mc.Add (0);
				mc.Add (2);
				break;
			case 'B':
				mc.Add (1);
				mc.Add (0);
				mc.Add (1);
				mc.Add (0);
				mc.Add (0);
				mc.Add (1);
				mc.Add (2);
				break;
			case 'C':
				mc.Add (1);
				mc.Add (1);
				mc.Add (1);
				mc.Add (0);
				mc.Add (1);
				mc.Add (0);
				mc.Add (2);
				break;
			case 'D':
				mc.Add (1);
				mc.Add (1);
				mc.Add (0);
				mc.Add (1);
				mc.Add (1);
				mc.Add (0);
				mc.Add (2);
				break;
			case 'E':
				mc.Add(0);
				mc.Add(1);
				mc.Add (0);
				mc.Add (0);
				mc.Add (0);
				mc.Add (0);
				mc.Add (2);
				break;
			case 'F':
				mc.Add(1);
				mc.Add(1);
				mc.Add(0);
				mc.Add(1);
				mc.Add (0);
				mc.Add (0);
				mc.Add (2);
				break;
			case 'G':
				mc.Add(1);
				mc.Add(0);
				mc.Add(1);
				mc.Add (0);
				mc.Add (1);
				mc.Add (1);
				mc.Add (2);
				break;
			case 'H':
				mc.Add(1);
				mc.Add(1);
				mc.Add(1);
				mc.Add(1);
				mc.Add (0);
				mc.Add (0);
				mc.Add (2);
				break;
			case 'I':
				mc.Add(0);
				mc.Add(1);
				mc.Add(1);
				mc.Add(0);
				mc.Add (0);
				mc.Add (0);
				mc.Add (2);
				break;
			case 'J':
				mc.Add(1);
				mc.Add(0);
				mc.Add(1);
				mc.Add(1);
				mc.Add (0);
				mc.Add (1);
				mc.Add (2);
				break;
			case 'K':
				mc.Add(1);
				mc.Add(0);
				mc.Add(1);
				mc.Add (1);
				mc.Add (1);
				mc.Add (1);
				mc.Add (2);
				break;
			case 'L':
				mc.Add(1);
				mc.Add(0);
				mc.Add(1);
				mc.Add(1);
				mc.Add (0);
				mc.Add (0);
				mc.Add (2);
				break;
			case 'M':
				mc.Add(1);
				mc.Add(1);
				mc.Add (1);
				mc.Add (1);
				mc.Add (1);
				mc.Add (0);
				mc.Add (2);
				break;
			case 'N':
				mc.Add(1);
				mc.Add(1);
				mc.Add (1);
				mc.Add (0);
				mc.Add (0);
				mc.Add (1);
				mc.Add (2);
				break;
			case 'O':
				mc.Add(1);
				mc.Add(0);
				mc.Add(0);
				mc.Add (1);
				mc.Add (0);
				mc.Add (1);
				mc.Add (2);
				break;
			case 'P':
				mc.Add(1);
				mc.Add(0);
				mc.Add(0);
				mc.Add(1);
				mc.Add (0);
				mc.Add (0);
				mc.Add (2);
				break;
			case 'Q':
				mc.Add(1);
				mc.Add(1);
				mc.Add(0);
				mc.Add(0);
				mc.Add (1);
				mc.Add (1);
				mc.Add (2);
				break;
			case 'R':
				mc.Add(1);
				mc.Add(1);
				mc.Add(1);
				mc.Add (0);
				mc.Add (0);
				mc.Add (0);
				mc.Add (2);
				break;
			case 'S':
				mc.Add(1);
				mc.Add(0);
				mc.Add(0);
				mc.Add (1);
				mc.Add (1);
				mc.Add (1);
				mc.Add (2);
				break;
			case 'T':
				mc.Add(1);
				mc.Add (1);
				mc.Add (0);
				mc.Add (1);
				mc.Add (0);
				mc.Add (1);
				mc.Add (2);
				break;
			case 'U':
				mc.Add(1);
				mc.Add(1);
				mc.Add(0);
				mc.Add (1);
				mc.Add (1);
				mc.Add (1);
				mc.Add (2);
				break;
			case 'V':
				mc.Add(1);
				mc.Add(0);
				mc.Add(1);
				mc.Add(0);
				mc.Add (1);
				mc.Add (0);
				mc.Add (2);
				break;
			case 'W':
				mc.Add(1);
				mc.Add(0);
				mc.Add(0);
				mc.Add (1);
				mc.Add (1);
				mc.Add (0);
				mc.Add (2);
				break;
			case 'X':
				mc.Add(1);
				mc.Add(1);
				mc.Add(1);
				mc.Add(0);
				mc.Add (1);
				mc.Add (1);
				mc.Add (2);
				break;
			case 'Y':
				mc.Add(1);
				mc.Add(1);
				mc.Add(1);
				mc.Add(1);
				mc.Add (0);
				mc.Add (1);
				mc.Add (2);
				break;
			case 'Z':
				mc.Add(1);
				mc.Add(0);
				mc.Add(1);
				mc.Add(1);
				mc.Add (1);
				mc.Add (0);
				mc.Add (2);
				break;
			}
		}
		return mc.ToArray();
	}

	IEnumerator Flashes()
	{
		while (!_isSolved)
		{
			flashRenderer1 = flashDisplay.GetComponent<Renderer>();
			flashRenderer1.enabled = true;
			for (int i = 0; i < encodedMessage.Length; i++) 
			{
				string[] flashColors = {"BLACK", "WHITE", "ORANGE"};
				if (_isSolved)
				{
					flashRenderer1.sharedMaterial = flashMat [2];
					if (colorblindSupportOn == true) 
					{
						colorblindText [1].text = flashColors [2];
					}
					break;
				}
				for (int j = 0; j < 3; j++) 
				{
					if (_isSolved)
					{
						flashRenderer1.sharedMaterial = flashMat [2];
						colorblindText [1].text = flashColors [2];
						break;
					}
					if (encodedMessage[i] == j) 
					{
						flashRenderer1.sharedMaterial = flashMat[j];
						if (colorblindSupportOn == true) 
						{
							colorblindText [1].text = flashColors[j];
						}
						else
						{
							colorblindText [1].text = "";
						}
					}
				}
				if (_isSolved)
				{
					flashRenderer1.sharedMaterial = flashMat [2];
					colorblindText [1].text = flashColors [2];
					break;
				}

				yield return new WaitForSeconds (1f);
			}

			flashRenderer1.sharedMaterial = flashMat [2];

			yield return new WaitForSeconds(4f);
		}
	}

	void buttonsMessage(KMSelectable pressedButton)
	{
		audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
		int buttonPosition = Array.IndexOf(messageButtons, pressedButton);

		if (_isSolved == false && flashing == true)
		{
			incorrect = false;
			switch (buttonPosition) 
			{
			case 0:
				//A dot
				if (intSubmitAnswers [curChar] == 1)
				{
					inputScreen.text += ".";
					curChar++;
				}
				else
				{
					if (intSubmitAnswers [curChar] == 2) 
					{
						module.HandlePass ();
						_isSolved = true;
						Log ("Solved!");
					}
					else
					{
						incorrect = true;
						Log ("Wrong character");
					}
				}
				break;
			case 1:
				//A slash
				if (intSubmitAnswers [curChar] == 0)
				{
					inputScreen.text += "/";
					curChar++;
				}
				else
				{
					if (intSubmitAnswers [curChar] == 2) 
					{
						module.HandlePass ();
						_isSolved = true;
						Log ("Solved!");
					}
					else
					{
						incorrect = true;
						Log ("Wrong character");
					}
				}
				break;
			}
			if (incorrect) 
			{
				module.HandleStrike ();
				curChar = 0;
				inputScreen.text = "";
			}
		}
	}

	void buttonsHertz(KMSelectable pressedButton)
	{
		audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
		int buttonPosition = Array.IndexOf(hertzButtons, pressedButton);

		if (_isSolved == false && flashing == false)
		{
			incorrect = false;
			switch (buttonPosition)  
			{
			case 0:
				if (hertzChooserNum == -1) 
				{
					return;
				}
				if (hertzNum == chooserTable[hertzChooserNum] && flashing == false) 
				{
					message = "";
					messageInt = Rnd.Range (0, 50);
					message = morseWords [messageInt];
					encodedMessage = Encode (message);
					Debug.LogFormat("[Hertz #{0}] Word is {1}", ModuleId, message);

					messageToInt ();

					//flash triggers
					Log ("Flash starting.");
					StartCoroutine (Flashes ());

					flashing = true;
				}
				else
				{
					incorrect = true;
				}
				break;
			case 1:
				hertzChooserNum++;
				hertzChooserNum = hertzChooserNum % 19;
				hertzChooserText.text = chooserTable [hertzChooserNum].ToString ();
				Debug.LogFormat ("[Hertz #{0}] Input hertz num switched to {1}.", ModuleId, chooserTable[hertzChooserNum]);
				break;
			}
			if (incorrect) 
			{
				module.HandleStrike ();
			}
		}
	}



	void Log(string message)
	{
		Debug.LogFormat("[Hertz #{0}] {1}", ModuleId, message);
	}

#pragma warning disable 414
	private string TwitchHelpMessage = "!{0} colorblind [toggles colorblind mode], !{0} submit ./././ [Submits a sequence of .'s and /'s], !{0} hertz ### (tries to cycle to the specified hertz number and then presses the transmit button).";
#pragma warning restore 414
	IEnumerator ProcessTwitchCommand(string command)
	{
		var colorblindMatch = Regex.Match(command, @"^colou?rblind$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
		var hertzFreqMatch = Regex.Match(command, @"^hertz\s\d{3}$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
		var submitMatch = Regex.Match(command, @"^submit\s[\./]{6}$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
		if (colorblindMatch.Success) 
		{
			yield return null;
			colorblindSupportOn = !colorblindSupportOn;
			colorblind ();
			yield break;
		}
		else if (submitMatch.Success)
        {
			var messagePart = submitMatch.Value.Split().Last();
			incorrect = false;
            for (var x = 0; x < messagePart.Length && !incorrect; x++)
            {
				yield return null;
				messageButtons[messagePart[x] == '.' ? 0 : 1].OnInteract();
				yield return new WaitForSeconds(0.1f);
            }
			if (!incorrect)
			{
				messageButtons.PickRandom().OnInteract();
			}
		}
		else if (hertzFreqMatch.Success)
		{
			var selectedHertz = hertzFreqMatch.Value.Split().Last();
			var convertedHertz = int.Parse(selectedHertz);
			if (!chooserTable.Contains(convertedHertz))
            {
				yield return "sendtochaterror The selected hertz " + selectedHertz + " cannot be accessed on this module!";
				yield break;
            }
			for (var x = 0; x < chooserTable.Length && convertedHertz != chooserTable.ElementAtOrDefault(hertzChooserNum); x++)
			{
				yield return null;
				hertzButtons[1].OnInteract();
				yield return new WaitForSeconds(0.1f);
			}

			yield return null;
			hertzButtons[0].OnInteract();
		}
	}

	void colorblind() 
	{
		if (colorblindSupportOn) 
		{
			string[] colors = {"BK", "G", "O", "R", "W", "BE", "P"};
			for (int i = 0; i < 7; i++)
			{
				if (colorRenderer1.sharedMaterial == colorMat[i]) 
				{
					colorblindText[0].text = colors[i]; 
				}
				if (colorRenderer2.sharedMaterial == colorMat[i]) 
				{
					colorblindText[2].text = colors[i]; 
				}
			}
			colorblindText[1].text = "ORANGE";
		}
		else
		{
			colorblindText [0].text = "";
			colorblindText [1].text = "";
			colorblindText [2].text = "";
		}
	}
}
