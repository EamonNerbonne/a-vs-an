defmodule AvsAn.MixProject do
  use Mix.Project

  @version "3.0.1"
  @git_url "https://github.com/EamonNerbonne/a-vs-an"

  def project do
    [
      app: :avs_an,
      version: @version,
      elixir: "~> 1.11",
      start_permanent: Mix.env() == :prod,
      deps: [{:ex_doc, "~> 0.27", only: :dev, runtime: false}]
    ]
  end

  defp docs do
    [
      main: "AvsAn",
      source_ref: "v#{@version}",
      source_url: @git_url
    ]
  end

  defp package do
    %{
      licenses: ["Apache-2.0"],
      maintainers: ["David Howland"],
      links: %{"GitHub" => @git_url}
    }
  end
end
