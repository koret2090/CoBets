import urllib.request
import json

FILENAME_KEY = 'api.key'
FILENAME_COEFS = 'coefs.csv'

BASE_URL = 'https://api.the-odds-api.com/v4/sports/'
SPORT = 'soccer_uefa_champs_league/'
SERVICE_NAME = 'odds/'
KEY_FORMAT = '?apiKey='
REGIONS = '&regions=eu'
MARKETS = '&markets=h2h'


#Формирование запроса и получение json-данных
def get_api_key(filename):
    f = open(filename, 'r')
    key = f.readline().strip()
    f.close()

    return key

def form_url():
    url = BASE_URL + SPORT + SERVICE_NAME
    url = url + KEY_FORMAT + get_api_key(FILENAME_KEY)
    url = url + REGIONS + MARKETS
    
    return url

def get_json_data():
    url = form_url()
    response = urllib.request.urlopen(url).read().decode()
    json_data = json.loads(response)
    
    return json_data


#Обработка json и выбор лучших коэффициентов
def get_bookmakers_data(team_home, bookmakers):
    books_data = []
    for bookmaker in bookmakers:
        name = bookmaker['title']
        coef1 = bookmaker['markets'][0]['outcomes'][0]['price']
        coef2 = bookmaker['markets'][0]['outcomes'][1]['price']
        
        if (team_home != bookmaker['markets'][0]['outcomes'][0]['name']):
            coef1, coef2 = coef2, coef1
        books_data.append((name, coef1, coef2))

    return books_data

def get_best_book_pair(books):
    books.sort(key=lambda coef: coef[1], reverse=True)
    book1 = books[0][0], books[0][1]

    books.sort(key=lambda coef: coef[2], reverse=True)
    book2 = books[0][0], books[0][2]

    return book1, book2

def get_data_coefs():
    games_full_data = get_json_data()
    num_games = sum([1 for game in games_full_data])
    
    data = []
    for i in range(num_games):
        time = games_full_data[i]['commence_time']
        team1 = games_full_data[i]['home_team']
        team2 = games_full_data[i]['away_team']
        
        bookmakers = games_full_data[i]['bookmakers']
        bookmakers_data = get_bookmakers_data(team1, bookmakers)

        book1, book2 = get_best_book_pair(bookmakers_data)
        data.append((time, team1, team2, (book1, book2)))

    return data


def write_data_in_file(data, filename):
    f = open(filename, "w")

    for game in data: 
        team1 = game[0] 
        team2 = game[1]
        
        book1 = game[2] 
        book2 = game[3]

        game_str = str(team1) + ',' + str(team2) + ',' +\
                   str(book1) + ',' + str(book2) + '\n'
        game_str = game_str.replace("(", "")
        game_str = game_str.replace(")", "")

        f.write(game_str)

    f.close()


def main():
    data_coefs = get_data_coefs()
    write_data_in_file(data_coefs, FILENAME_COEFS)
    print('Data was written to the file!')

if __name__ == "__main__":
    main()
