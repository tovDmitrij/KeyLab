from os import walk

kitFolders = [
    'd296c943-4894-484a-b0c3-9b3783accbaa',
    'b00b2fd6-5dac-4994-b753-c4897d161561',
    '3f3b4d25-d2f5-4194-aec9-2ee17f61cbe0',
    '3d3f4e56-8ed2-4f85-84ef-8b2325e989ff'
]

with open('keycaps.csv', 'w') as kitFile:
    kitFile.write('kit_id,title,file_name,preview_name,creation_date')

    for folder in kitFolders:
        filenames = next(walk(folder), (None, None, []))[2]
        for filename in filenames:
            splitedFilename = filename.split('.')[0]
            str = '\n{kit_id},{title},{file_name},{preview_name},{creation_date}'.format(
                kit_id=folder, 
                title=splitedFilename.title().replace('_', ''), 
                file_name=filename,
                preview_name=splitedFilename + '.jpeg',
                creation_date='1706026855')
            kitFile.write(str)