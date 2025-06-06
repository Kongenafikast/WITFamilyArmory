using MySql.Data.MySqlClient;
using static WITFamilyArmory.Models.Inventory;

namespace WITFamilyArmory.Repository
{
    public class DatabaseRepo
    {
        private readonly string _connectionString;
        public DatabaseRepo(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<Weapon> GetWeapons(string query)
        {
            List<Weapon> weapons = new List<Weapon>();
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        weapons.Add(new Weapon()
                        {
                            Id = reader.GetInt32("id"),

                            Item = reader["weaponName"] == DBNull.Value ? "" : reader["weaponName"].ToString(),
                            Type = reader["weaponType"] == DBNull.Value ? "" : reader["weaponType"].ToString(),
                            Bouns1 = reader["weaponBouns1"] == DBNull.Value ? "" : reader["weaponBouns1"].ToString(),
                            Bouns1pct = reader["weaponBouns1pct"] == DBNull.Value ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("weaponBouns1pct")),
                            Bouns2 = reader["weaponBouns2"] == DBNull.Value ? "" : reader["weaponBouns2"].ToString(),
                            Bouns2pct = reader["weaponBouns2pct"] == DBNull.Value ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("weaponBouns2pct")),
                            Quality = reader["quality"] == DBNull.Value ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("quality")),
                            Damage = reader["accuracy"] == DBNull.Value ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("accuracy")),
                            Accuracy = reader["damage"] == DBNull.Value ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("damage")),
                            Lokation = new FactionInfo
                            {
                                Name = reader["factionName"] == DBNull.Value ? "" : reader["factionName"].ToString(),
                                Id = reader.GetInt32("factionId")
                            },
                            Owner = reader["owner"] == DBNull.Value ? "" : reader["owner"].ToString()
                        });
                    }
                }
                connection.Close();
            }

            return weapons;
        }
        public List<Armor> GetArmors(string query)
        {
            List<Armor> armors = new List<Armor>();
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        armors.Add(new Armor()
                        {
                            Id = reader.GetInt32("id"),
                            Item = reader["armorName"] == DBNull.Value ? "" : reader["armorName"].ToString(),
                            Bonus = reader["armorBonus"] == DBNull.Value ? "" : reader["armorBonus"].ToString(),
                            Bounspct = reader["armorBonuspct"] == DBNull.Value ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("armorBonuspct")),
                            Quality = reader["quality"] == DBNull.Value ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("quality")),
                            ArmorPct = reader["armor"] == DBNull.Value ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("armor")),
                            Coverage = reader["coverage"] == DBNull.Value ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("coverage")),
                            Lokation = new FactionInfo
                            {
                                Name = reader["factionName"] == DBNull.Value ? "" : reader["factionName"].ToString(),
                                Id = reader.GetInt32("factionId")
                            },
                            Owner = reader["owner"] == DBNull.Value ? "" : reader["owner"].ToString()
                        });
                    }
                }
                connection.Close();
            }

            return armors;
        }
        public void CreateItem(string query)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand(query, connection))
                {
                    var test = command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
        public void DeleteItem(string query)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
        public List<FactionInfo> GetFactions()
        {
            List<FactionInfo> factions = new List<FactionInfo>();
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand("SELECT * FROM factionInfo", connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        factions.Add(new FactionInfo()
                        {
                            Id = reader.GetInt32("factionId"),
                            Name = reader["factionName"] == DBNull.Value ? "" : reader["factionName"].ToString()
                        });

                    }
                }
                connection.Close();
            }
            return factions;
        }
        public void UpdateItem(string query)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
        public userInfo GetUserInfo(string query)
        {
            List<userInfo> userInfos = new List<userInfo>();
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        userInfos.Add(new userInfo()
                        {
                            ID = reader.GetInt32("iD"),
                            Username = reader["tornUsername"] == DBNull.Value ? "" : reader["tornUsername"].ToString(),
                            faction = new FactionInfo
                            {
                                Name = reader["factionName"] == DBNull.Value ? "" : reader["factionName"].ToString(),
                                Id = reader.GetInt32("factionId")
                            },
                            TornId = reader.GetInt32("tornId"),
                            Password = reader["hashedPassword"] == DBNull.Value ? "" : reader["hashedPassword"].ToString()
                        });
                    }
                }
                connection.Close();
            }
            if (userInfos.Count > 0)
            {
                return userInfos[0];
            }
            return null;
        }
    }
}
