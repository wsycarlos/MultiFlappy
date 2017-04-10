namespace csharp Bird

struct Place{
1:double x,
2:double y,
}

service Flappy {

   i32 session(),
   
   map<i32,Place> start(),
   
   map<i32,Place> waitstart(),

   void move(1:i32 ses, 2:Place place),
   
   void crash(1:i32 ses),
   
   map<i32,Place> msync(),
   
   list<i32> nsync()
}