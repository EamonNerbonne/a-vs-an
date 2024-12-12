""" Determines whether to use 'a' or 'an' """

import json
from pathlib import Path
from typing import Self


class AvsAn:
    """
    Singleton class, AvsAn.

    ```python
    avsan = AvsAn.getInstance()
    article = avsan.query("honest decision")
    ```
    """

    __instance = None
    __root: dict[str, dict]

    @staticmethod
    def getInstance() -> Self:
        """ Static access method. """
        if AvsAn.__instance is None:
            AvsAn()
        return AvsAn.__instance

    def __init__(self):
        """ Virtually private constructor. """
        if AvsAn.__instance is not None:
            raise Exception("This class is a singleton!")
        else:
            path = Path(__file__).parent / "a_vs_an.json"
            with path.open() as f:
                self.__root = json.load(f)
            AvsAn.__instance = self

    def query(self, word: str) -> dict[str, int | str]:
        """ Return article info for a given word. """
        word = word.lstrip("'\"`-($")
        node = self.__root
        for char in word:
            try:
                node = node[char]
            except KeyError:
                break
        data: dict[str, int | str] = node["data"]
        return data
