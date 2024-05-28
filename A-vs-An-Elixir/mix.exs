defmodule AvsAn.MixProject do
  use Mix.Project

  @version "3.0.1"

  def project do
    [
      app: :avs_an,
      version: @version,
      elixir: "~> 1.11",
      start_permanent: Mix.env() == :prod,
      deps: []
    ]
  end
end
