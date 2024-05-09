from generator import *
import csv
import math
import time



def initFakeData(fileName: str, limit: int, fields: list[str], defaultData: list[list[str]], funcWithGeneratingData):
    start = time.time()

    with open(f'{fileName}', 'w', newline='') as file:
        fileWriter = csv.writer(file)
        fileWriter.writerow(fields)

        for data in defaultData:
            fileWriter.writerow(data)
        
        for i in range(0, limit):
            print(f'{i}/{limit}')

            fileWriter.writerow(funcWithGeneratingData())
    
    end = time.time()
    seconds = end - start
    print(f'Генерация данных для файла {fileName} длины {limit} заняло {math.ceil(seconds)} секунд!')



def generateTableUsers():
    ''' db_users '''
    fields = ["id", "email", "salt", "password", "nickname", "registration_date"]
    defaultData = [
        ["dced1acd-b907-47e0-9659-77cb2c95e0aa", "admin@keyboard.ru", "hL36LBnKvP8whM0QFhuFQn82GSBJbPXT", "72653061b3aefd326e2e71f7affc9ccd1d1473fc6e35d5b4936d87c587b96dcff1729b65a57e7aaf95b964ac325fac56d7ef626cb9ea4fcad0287045176ed96e", "admin1", "1705443770.40067"]
    ]
    #initFakeData('./users/init/users_10_000.csv', 10_000, fields, defaultData, generateTableUsersFields)
    #initFakeData('./users/init/users_100_000.csv', 100_000, fields, defaultData, generateTableUsersFields)
    #initFakeData('./users/init/users_1_000_000.csv', 1_000_000, fields, defaultData, generateTableUsersFields) #~3 минуты и ~111мб
    #initFakeData('./users/init/users_5_000_000.csv', 5_000_000, fields, defaultData, generateTableUsersFields)
    #initFakeData('./users/init/users_10_000_000.csv', 10_000_000, fields, defaultData, generateTableUsersFields) #~30 минут и ~1,1гб

generateTableUsers()