#usage

```json
//ko-kr.json
{
	TITLE : "1차 드라이버"
}

//en-US.json
{
	TITLE : "1st driver"
}
```

``` cs
public class LangageResource {
	public string TITLE = "1st driver";

	///.... and so on;
}

///....

public class Test {
	private LangageHandler<LanguageResource> lang = LanguageHandler.Instance["ko-KR"];
	//private LangageHandler<LanguageResource> lang = LanguageHandler.Instance["en-US"];
	public Test() {
		this.Textbox1.text = lang.TITLE; //if ko-kr is Korean, 1차 드라이버; if en-US is English, 1st driver;
	}
}
```
