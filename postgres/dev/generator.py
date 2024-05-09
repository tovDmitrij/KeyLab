from uuid import UUID, uuid4
import string
import random



def generateID() -> UUID:
    return uuid4()

def generateEmail() -> str:
    return ''.join(random.choice(string.ascii_uppercase + string.digits) for _ in range(6)) + '@gmail.com'

def generateSalt() -> str:
    return f'{(int(''.join(random.choice(string.digits) for _ in range(32)))):x}'

def generatePassword() -> str:
    return f'{(int(''.join(random.choice(string.digits) for _ in range(128)))):x}'

def generateNickname() -> str:
    return ''.join(random.choice(string.ascii_uppercase + string.digits) for _ in range(10))

def generateRegistrationDate() -> int:
    return int(''.join(random.choice(string.digits) for _ in range(10)))



def generateTableUsersFields() -> list[str]:
    return [
        generateID(), generateEmail(), generateSalt(), 
        generatePassword(), generateNickname(), generateRegistrationDate()
    ]