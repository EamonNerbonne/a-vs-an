import json
import os

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
            __location__ = os.path.realpath(os.path.join(os.getcwd(), os.path.dirname(__file__)))
            with open(os.path.join(__location__, 'a_vs_an.json')) as f:

                self.root = json.load(f)
                print("a_vs_an.json was loaded")
            AvsAn.__instance = self

    def query(self, word):
        node = self.root
        sI = 0
        c = ''
        while True:
            if (sI >= len(word)):
                break
            c = word[sI]
            sI = sI + 1
            if c not in "'\"`-($":
                break

        result = None
        while True:
            result = node['data']

            if c not in node:
                return result

            node = node[c]

            if sI >= len(word):
                c = " "
            else:
                c = word[sI]
            sI = sI + 1

