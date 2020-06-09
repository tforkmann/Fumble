module ChiaRedisCache

open Feliz
open Feliz.Bulma
open Shared

let overview =
    Html.div
        [ Bulma.title.h1 [ Html.text "Chia.RedisCache" ]
          Bulma.subtitle.h2
              [ Html.text "Helper to create or directly query a RedisCache" ]
          Html.hr []
          Bulma.content
              [ Html.p "To create or read a Redis values with a Redis Key you first have to create a Redis cache info:"
                code """
                let cacheInfo : RedisCache = {
                    Cache = Redis.cache
                    Key = key
                    FileWriterInfo = fileWriterInfo }"""
                Html.p "To deserialze your Redis values to your pass in a System.Text.Json mapper."
                Html.p "You also should pass in a task to receive your data. The function tries to find the cache in Redis."
                Html.p "If there is no Redis cache it will create a new cache by executing you task."
                Html.p "The following example showes how to reveice a a Plant array directly out of Redis or creates a new cache if theres no existing cache and returns the Plant array."
      ] ]
