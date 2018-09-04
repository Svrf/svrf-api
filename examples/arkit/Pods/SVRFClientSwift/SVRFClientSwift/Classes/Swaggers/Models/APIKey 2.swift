//
// APIKey.swift
//
// Generated by swagger-codegen
// https://github.com/swagger-api/swagger-codegen
//

import Foundation



open class APIKey: Codable {

    public var id: String
    public var key: String
    public var name: String
    public var type: Int?


    
    public init(id: String, key: String, name: String, type: Int?) {
        self.id = id
        self.key = key
        self.name = name
        self.type = type
    }
    

    // Encodable protocol methods

    public func encode(to encoder: Encoder) throws {

        var container = encoder.container(keyedBy: String.self)

        try container.encode(id, forKey: "id")
        try container.encode(key, forKey: "key")
        try container.encode(name, forKey: "name")
        try container.encodeIfPresent(type, forKey: "type")
    }

    // Decodable protocol methods

    public required init(from decoder: Decoder) throws {
        let container = try decoder.container(keyedBy: String.self)

        id = try container.decode(String.self, forKey: "id")
        key = try container.decode(String.self, forKey: "key")
        name = try container.decode(String.self, forKey: "name")
        type = try container.decodeIfPresent(Int.self, forKey: "type")
    }
}

