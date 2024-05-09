from uuid import UUID, uuid4
import csv
import string
import random
import asyncio



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



def generateFakeDataFile():
    with open('users_100million.csv', 'w', newline='') as file:
        fields = ["id", "email", "salt", "password", "nickname", "registration_date"]
        fileWriter = csv.writer(file)
        fileWriter.writerow(fields)

        fileWriter.writerow(["dced1acd-b907-47e0-9659-77cb2c95e0aa", "admin@keyboard.ru", "hL36LBnKvP8whM0QFhuFQn82GSBJbPXT", "72653061b3aefd326e2e71f7affc9ccd1d1473fc6e35d5b4936d87c587b96dcff1729b65a57e7aaf95b964ac325fac56d7ef626cb9ea4fcad0287045176ed96e", "admin1", "1705443770.40067"])
        
        limit = 1_000_000
        for i in range(0, limit):
            print(f'{i}/{limit}')

            fileWriter.writerow([generateID(), generateEmail(), generateSalt(), generatePassword(), generateNickname(), generateRegistrationDate()])

generateFakeDataFile()