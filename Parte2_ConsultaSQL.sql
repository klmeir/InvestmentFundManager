-- Índices recomendados:
--   - Inscripcion(idCliente, idProducto)
--   - Disponibilidad(idProducto, idSucursal)
--   - Visitan(idCliente, idSucursal)

SELECT DISTINCT c.nombre, c.apellidos
FROM Cliente c
INNER JOIN Inscripcion i ON c.id = i.idCliente
INNER JOIN Producto p ON i.idProducto = p.id
WHERE NOT EXISTS (
    -- Verificar que NO exista ninguna sucursal donde:
    -- 1. El producto esté disponible
    -- 2. El cliente NO visite esa sucursal
    SELECT 1
    FROM Disponibilidad d
    WHERE d.idProducto = p.id
    AND NOT EXISTS (
        -- El cliente NO visita esta sucursal
        SELECT 1
        FROM Visitan v
        WHERE v.idCliente = c.id
        AND v.idSucursal = d.idSucursal
    )
)
AND EXISTS (
    -- Asegurar que el producto esté disponible en al menos una sucursal que el cliente visita
    SELECT 1
    FROM Disponibilidad d2
    INNER JOIN Visitan v2 ON d2.idSucursal = v2.idSucursal
    WHERE d2.idProducto = p.id
    AND v2.idCliente = c.id
)
ORDER BY c.nombre, c.apellidos;
