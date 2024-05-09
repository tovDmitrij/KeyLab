from uuid import UUID, uuid4
import csv
import string
import random
import time



def generateID() -> UUID:
    return uuid4()

def generateEmail() -> str:
    return ''.join(random.choice(string.ascii_uppercase + string.digits) for _ in range(6)) + '@gmail.com'

def generateSalt() -> str:
    return f'{(int(''.join(random.choice(string.digits) for _ in range(32)))):x}'

def generatePassword() -> str:
    return f'{(int(''.join(random.choice(string.digits) for _ in range(10)))):x}'

def generateNickname() -> str:
    return ''.join(random.choice(string.ascii_uppercase + string.digits) for _ in range(10))

def generateRegistrationDate() -> int:
    return int(''.join(random.choice(string.digits) for _ in range(10)))



def generateFakeData(fileName: str, limit: int):
    start = time.time()

    with open(f'{fileName}', 'w', newline='') as file:
        fields = ["id", "email", "salt", "password", "nickname", "registration_date"]
        fileWriter = csv.writer(file)
        fileWriter.writerow(fields)

        fileWriter.writerow(["dced1acd-b907-47e0-9659-77cb2c95e0aa", "admin@keyboard.ru", "hL36LBnKvP8whM0QFhuFQn82GSBJbPXT", "72653061b3aefd326e2e71f7affc9ccd1d1473fc6e35d5b4936d87c587b96dcff1729b65a57e7aaf95b964ac325fac56d7ef626cb9ea4fcad0287045176ed96e", "admin1", "1705443770.40067"])
        
        for i in range(0, limit):
            print(f'{i}/{limit}')

            fileWriter.writerow([
                generateID(), generateEmail(), generateSalt(), 
                generatePassword(), generateNickname(), generateRegistrationDate()
            ])
    
    end = time.time()
    seconds = end - start
    print(f'Генерация данных для файла {fileName} длины {limit} заняло {seconds} секунд!')



'''
db_users
'''
#generateFakeData('./users/init/users_1million.csv', 1_000_000) #~3 минуты и ~111мб
#generateFakeData('./users/init/users_10million.csv', 10_000_000) #~30 минут и ~1,1гб
#generateFakeData('./users/init/users_30million.csv', 30_000_000)