from bs4 import BeautifulSoup as bs
import requests
import sqlite3

url = ""

headers = {
	'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/65.0.3325.181'
}

data = {
	'username': '',
	'password': ''
}

session = requests.Session()
rr = session.post(url, headers=headers, data=data)
html = rr.text

soup = bs(html, 'html.parser')
titleList = []
dateList = []
linkList = []

for a in soup.find_all('ul', class_='unlist'):
	info = soup.find_all("div", class_="info")
	for data in info:
	    if data.a:
	        text = data.a.text
	        link = data.a.get("href")
	       	titleList.append(text)
	       	linkList.append(link)

for a in soup.find_all('ul', class_='unlist'):
	info = soup.find_all("div", class_="head clearfix")
	for data in info:
	    if data.div:
	        text = data.div.text
	        dateList.append(text)
dbname = "mailsender.db"
con = sqlite3.connect(dbname)
sql = con.cursor()
sql.execute('DROP TABLE IF EXISTS haber')
sql.execute('CREATE TABLE IF NOT EXISTS haber(haber_kimligi INTEGER PRIMARY KEY, baslik TEXT, tarih TEXT, link TEXT)')
for i in range(len(titleList) - 1):
	sql.execute('INSERT INTO haber(baslik, tarih, link) VALUES(?, ?, ?)', (titleList[i], dateList[i], linkList[i]))
	i += 1
con.commit()
con.close()


























# this section was for just simple tests.

""" get author name
for a in soup.find_all('ul', class_='unlist'):
	name = soup.find_all("div", class_="name")
	for data in name:
		if data.div:
			text = data.div.text
			authorList.append(text)
print(authorList)
"""


"""urll = "https://onedio.com/haberler"

headers = {
	'User-Agent': 'Mozilla/5.0'
}


req = Request(urll)
req.add_header('User-Agent', 'Mozilla/5.0')

with urlopen(req) as url:
	html = url.read().decode("UTF-8")
	soup = bs(html, "html.parser")

for a in soup.find_all("h3", {"class": "o-card-title"}):
	b = re.sub(r"[<h3>,</h3>]", "", str(a))
	print(re.sub(r"[class='o-card-title', '']", "", str(b)))
"""
