from email.policy import default
import requests
from bs4 import BeautifulSoup as BS

teams_page_start = 1
teams_page_end = 978

cite_team = "https://www.liveresult.ru/football/teams/p"
cite_players = "https://www.liveresult.ru/football/teams/"

filename_players = "players.csv"
filename_teams = "teams.csv"

default_num = 322
default_pos = 9

def get_teams_info():
    titles = []
    for page_num in range(teams_page_start, teams_page_end):
        try:
            print(page_num)
            r = requests.get(cite_team + str(page_num))
            html = BS(r.content, 'html.parser')
            teams = html.find_all("h5", class_="card-title")
            for team in teams:
                title = get_name(team.find("a"))
                titles.append(title)
        except:
            continue

    return titles

def get_name(string):
    return string["href"].split("/")[3]

def write_teams(teams):
    f = open(filename_teams, "w")

    i = 0
    for team in teams:
        i += 1
        string = str(i) + "," + team + '\n' 
        f.write(string)

    f.close()

def get_teams_members_info_in_file(teams):
    f = open(filename_players, "w")
    
    i = 0
    j = 0
    for team in teams:
        print(team)
        try:
            i += 1
            r = requests.get(cite_players + str(team))
            html = BS(r.content, 'html.parser')
            players = html.find_all("td", class_="name left wrap")
            numbers = html.find_all("td", class_="num right")
            positions = html.find_all("td", class_="role")
            
            for index in range(len(players)):
                j += 1
                name = get_name(players[index].find("a"))                
                number = numbers[index].find("span").text
                pos = positions[index].find("span", class_="hidden d-none").text

                # при некорректном считывании присвоить такие значения
                if number == "": number = default_num
                if pos == "": pos = default_pos

                string = str(j) + "," + str(i) + "," + name + "," + str(number) + "," + str(pos) + '\n'
                f.write(string)
        except:
            continue

    f.close() 

def main():
    teams = get_teams_info()
    write_teams(teams)
    get_teams_members_info_in_file(teams)


if __name__ == "__main__":
    main()
