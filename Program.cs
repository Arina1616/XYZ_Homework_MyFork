namespace Projects
{
    class Program
    {
        public enum AttackType
        {
            Damage ,
            Self ,
            Heal 
        }
        static void Main()
        {   

            string? Name;
            float MaxPlayerHealth = 1000f; 
            float MaxEnemyHealth = 2000f; 

            float CurrentPLAYERHealth = MaxPlayerHealth;
            float CurrentENEMYHealth = MaxEnemyHealth;

            float playerDamage = 50f;
            float FireballDamage = 200f;
            float PlayerHeal = 100f; //ДЗ №3 ( исцеление игрока)

            float EnemyDamage = 55f;
            float EnemyHealDamage = 100f;
            bool EnemyDamageWithWeapon = false;

            int commandCount = 3;
            int enemySecondAbilityModifier = 3;

            Random Randomizer = new Random();

            Dictionary<AttackType , List <float>> damagesToPlayer = new();
            Dictionary<AttackType , List <float>> damagesToEnemy = new();
            damagesToPlayer[AttackType.Damage] = new List<float>();
            damagesToPlayer[AttackType.Self] = new List<float>();
            damagesToPlayer[AttackType.Heal] = new List<float>();

            damagesToEnemy[AttackType.Damage] = new List<float>();
            damagesToEnemy[AttackType.Self] = new List<float>();
            damagesToEnemy[AttackType.Heal] = new List<float>();


            string? Input;

            Console.WriteLine("Введите имя Игрока");
            Name = Console.ReadLine();

            Console.Clear();
            Console.WriteLine($"Добро пожаловать в игру , {Name}\n");

            while(CurrentPLAYERHealth > 0 || CurrentENEMYHealth > 0)
            {   
                if(CurrentPLAYERHealth > MaxPlayerHealth * 0.1 && CurrentENEMYHealth >  MaxEnemyHealth * 0.1) // если текущее значение здоровья больше 10% , значит окрашиваем в зеленный  
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                } 
                else 
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                Console.WriteLine($"Ваше здоровье : {CurrentPLAYERHealth}");
                Console.WriteLine($"Здоровье противника : {CurrentENEMYHealth }");
                Console.ResetColor();

                Console.WriteLine($"{Name} выберите действие :\n"
                 + $" 1. Ударить оружием (урон {playerDamage}) \n" +
                   $" 2. Щит: следующая атака не наносит урона \n" +
                   $" 3. Огненный шар: наносит урон в размере {FireballDamage} \n" + 
                   $" 4. Аптечка: Исцеление в размере {PlayerHeal} единиц \n");

                Input = Console.ReadLine();
                bool playerShield = false;     
                bool playerDamageWithWeapon = false;

                switch (Input)
                {
                    case "1": 
                        playerDamageWithWeapon = true;
                        CurrentENEMYHealth -= playerDamage; // Текущее здоровье Врага - 50 единиц
                        damagesToEnemy[AttackType.Damage].Add(playerDamage);
                        break;
                    case "2": 
                        playerShield = true;
                        Console.WriteLine("Вы использовали щит");
                        break;
                    case "3": 
                        CurrentENEMYHealth -= FireballDamage; // Текущее здоровье Врага - 200 единиц
                        damagesToEnemy[AttackType.Damage].Add(FireballDamage);
                        break;
                     case "4": //ДЗ №3 ( исцеление игрока )
                        if (!EnemyDamageWithWeapon & CurrentPLAYERHealth < MaxPlayerHealth )// если предыдущий урон врага был нанесен НЕ с оружия
                        { 
                            CurrentPLAYERHealth += PlayerHeal; // исцеляемся
                            damagesToEnemy[AttackType.Heal].Add(PlayerHeal);
                            EnemyDamageWithWeapon = false;
                            if (CurrentPLAYERHealth > MaxPlayerHealth)
                            {
                                CurrentPLAYERHealth = MaxPlayerHealth;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Исцеление действует лишь тогда , когда предыдущий урон противника был нанесен не оружием ");
                        }
                        break;    
                    case "Exit": 
                        Environment.Exit(0);
                        break;
                    default : 
                        Console.WriteLine("Команда не распознана!");
                        break;
                }
                int enemyComand = Randomizer.Next(1, commandCount + 1);

                switch (enemyComand)
                {
                    case 1: 
                        if (!playerShield)
                        {   
                            EnemyDamageWithWeapon = true; 
                            CurrentPLAYERHealth -= EnemyDamage; // Текущее здоровье Игрока - 55 единиц
                            damagesToPlayer[AttackType.Damage].Add(EnemyDamage);
                        }
                        break;
                    case 2: 
                        if (!playerShield)
                        {   
                            CurrentENEMYHealth -= EnemyDamage; // Текущее здоровье Врага (атакует сам себя ) - 55 единиц
                            CurrentPLAYERHealth -= EnemyDamage * enemySecondAbilityModifier ;   // Текущее здоровье Игрока -(55 * 3)=  - 165 единиц
                            damagesToPlayer[AttackType.Self].Add(EnemyDamage);
                            damagesToPlayer[AttackType.Damage].Add(EnemyDamage * enemySecondAbilityModifier);
                        } 
                        EnemyDamageWithWeapon = false;
                        break;
                    case 3: 
                        if (playerDamageWithWeapon)
                        {
                            CurrentENEMYHealth -= EnemyHealDamage; 
                            damagesToPlayer[AttackType.Self].Add(EnemyHealDamage);
                        }
                        else 
                        {
                            CurrentENEMYHealth += EnemyHealDamage;
                            damagesToPlayer[AttackType.Heal].Add(EnemyHealDamage);
                            // проверка здоровья на положительное максимальное значение 
                            if (CurrentENEMYHealth > MaxEnemyHealth )
                            {
                                CurrentENEMYHealth = MaxEnemyHealth; 
                            }
                        } 
                        EnemyDamageWithWeapon = false;
                        break;
                    default : 
                        Console.WriteLine ("Команда не распознана!");
                        break;
                }
                Console.ForegroundColor = ConsoleColor.Red;
                WriteDamageFromTo(damagesToEnemy , Name , "Enemy");
                WriteDamageFromTo(damagesToPlayer ,"Enemy" , Name);
                Console.ResetColor();

                AndTurn();
            }    

                void AndTurn()
                {
                    Console.WriteLine ("\nДля продолжения введите любую клавишу ");
                    Console.ReadKey();
                    Console.Clear();
                }


                void DamageMassange (string damager, string defender , float damage)
                {
                    Console.WriteLine($"{damager} наносит игроку {defender} урон в размере {damage} единиц");
                }

                void HealMassage (string healer , float HealValue )
                {
                    Console.WriteLine($"{healer} восстановил себе здоровье в размере {HealValue} единиц ");
                }

                void SelfMassange (string damager, float damage)
                {
                    Console.WriteLine($"{damager} наносит себе урон в размере {damage} единиц");
                }

                void WriteDamageWithType(AttackType attackType, string damagerName , string defenderName, List<float> damages)
                {
                    foreach(var damage in damages)
                    {
                        switch (attackType)
                        {
                            case AttackType.Damage:
                                DamageMassange(damagerName , defenderName , damage);
                                break;
                            case AttackType.Self:
                                SelfMassange(damagerName , damage);
                                break;
                            case AttackType.Heal:
                                HealMassage(damagerName , damage);
                                break;
                            default :
                                break;
                        }
                            
                    }
                    damages.Clear();
                }

                void WriteDamageFromTo (Dictionary<AttackType , List<float>> damages , string damagerName ,string defenderName )
                {
                    foreach (var damage in damages)
                    {
                        WriteDamageWithType(damage.Key , damagerName , defenderName , damage.Value);
                    }
                }
            }
        }
    }



