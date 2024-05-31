defmodule AvsAnTest do
  use ExUnit.Case

  test "article_for/1" do
    assert AvsAn.article_for("unanticipated result") == "an"
    assert AvsAn.article_for("unanimous vote") == "a"
    assert AvsAn.article_for("honest decision") == "an"
  end
end
