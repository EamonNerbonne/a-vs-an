
from AvsAn import AvsAn

result = AvsAn.getInstance().query("")
print result
result = AvsAn.getInstance().query("x")
print result
result = AvsAn.getInstance().query("\"x")
print result
result = AvsAn.getInstance().query("unanticipated result")
print result
result = AvsAn.getInstance().query("unanimous vote")
print result
result = AvsAn.getInstance().query("honest decision")
print result