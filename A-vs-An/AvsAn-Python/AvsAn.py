import json

class AvsAn():
    """
    Singleton class, AvsAn.
    """

    __instance = None

    @staticmethod
    def getInstance():
        """ Static access method. """
        if AvsAn.__instance == None:
            AvsAn()
        return AvsAn.__instance

    def __init__(self):
        """ Virtually private constructor. """
        if AvsAn.__instance != None:
            raise Exception("This class is a singleton!")
        else:
            with open('a_vs_an.json') as f:
                self.root = json.load(f)
                print("a_vs_an.json was loaded")
            AvsAn.__instance = self

    def query(self, word):
        node = self.root
        sI = 0
        c = ''
        while True:
            c = word[sI]
            sI = sI + 1
            if c not in "'\"`-($":
                break

        while True:
            result = node['data']

            if c not in node:
                return result

            node = node[c]

            c = word[sI]
            sI = sI + 1

