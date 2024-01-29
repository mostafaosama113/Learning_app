﻿namespace Learning_platform.Models
{
    public class Lesson
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Video { get; set; }
        public int Price { get; set; }
        public int Vote { get; set; }
        public Course Course { get; set; }
    }
}