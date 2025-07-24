namespace cslogo
{
    public record TilemapElement(string Type, int Row, int Col, string Value, int Width = 1, int Height = 1, string Tag = "");
}