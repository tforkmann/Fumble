module ChiaExcelUtils

open Feliz
open Feliz.Bulma
open Shared

let overview =
    Html.div
        [ Bulma.title.h1 [ Html.text "Chia.ExcelUtils" ]
          Bulma.subtitle.h2
              [ Html.text "Mini Excel Helper" ]
          Html.hr []
          Bulma.content
              [ Html.p "Mini Helper to start and ExcelApp using the EPPlus package."
                Html.p "Start your excel app like this:"
                code """
                let excelPackage = startExcelApp ()""" ] ]
